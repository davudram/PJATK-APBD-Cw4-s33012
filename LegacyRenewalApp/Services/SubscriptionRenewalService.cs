using System;
using System.Collections.Generic;
using LegacyRenewalApp.Interfaces;
using LegacyRenewalApp.Repositories;
using LegacyRenewalApp.Services;
using LegacyRenewalApp.Strategies.DiscountRules;
using LegacyRenewalApp.Strategies.PaymentFees;
using LegacyRenewalApp.Wrappers;

namespace LegacyRenewalApp
{
    public class SubscriptionRenewalService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ISubscriptionPlanRepository _planRepository;
        private readonly IDiscountCalculator _discountCalculator;
        private readonly FeeCalculatorService _feeCalculatorService;
        private readonly TaxCalculatorService _taxCalculatorService;
        private readonly IBillingGateway _billingGateway;

        public SubscriptionRenewalService() : this(
            new CustomerRepository(), 
            new SubscriptionPlanRepository(),
            new DiscountService(new List<IDiscountRule> 
            { 
                new EducationDiscountRule(), 
                new PlatinumDiscountRule(),
                new GoldDiscountRule(),
                new SilverDiscountRule(),
                new LargeTeamDiscountRule(),
                new MediumTeamDiscountRule(),
                new SmallTeamDiscountRule()
            }),
            new FeeCalculatorService(new List<IPaymentFeeStrategy>
            {
                new CardPaymentFeeStrategy(),
                new PayPalPaymentFeeStrategy(),
                new BankTransferPaymentFeeStrategy(),
                new InvoicePaymentFeeStrategy()
            }),
            new TaxCalculatorService(),
            new BillingGatewayWrapper())
        {
        }

        public SubscriptionRenewalService(
            ICustomerRepository customerRepository,
            ISubscriptionPlanRepository planRepository,
            IDiscountCalculator discountCalculator,
            FeeCalculatorService feeCalculatorService,
            TaxCalculatorService taxCalculatorService,
            IBillingGateway billingGateway)
        {
            _customerRepository = customerRepository;
            _planRepository = planRepository;
            _discountCalculator = discountCalculator;
            _feeCalculatorService = feeCalculatorService;
            _taxCalculatorService = taxCalculatorService;
            _billingGateway = billingGateway;
        }

        public RenewalInvoice CreateRenewalInvoice(
            int customerId, string planCode, int seatCount, string paymentMethod,
            bool includePremiumSupport, bool useLoyaltyPoints)
        {
            if (customerId <= 0) throw new ArgumentException("Customer id must be positive");
            if (string.IsNullOrWhiteSpace(planCode)) throw new ArgumentException("Plan code is required");
            if (seatCount <= 0) throw new ArgumentException("Seat count must be positive");
            if (string.IsNullOrWhiteSpace(paymentMethod)) throw new ArgumentException("Payment method is required");

            string normalizedPlanCode = planCode.Trim().ToUpperInvariant();
            string normalizedPaymentMethod = paymentMethod.Trim().ToUpperInvariant();

            var customer = _customerRepository.GetById(customerId);
            var plan = _planRepository.GetByCode(normalizedPlanCode);

            if (!customer.IsActive) throw new InvalidOperationException("Inactive customers cannot renew subscriptions");

            decimal baseAmount = (plan.MonthlyPricePerSeat * seatCount * 12m) + plan.SetupFee;

            var discountContext = new DiscountContext 
            { 
                Customer = customer, 
                Plan = plan, 
                SeatCount = seatCount, 
                BaseAmount = baseAmount, 
                UseLoyaltyPoints = useLoyaltyPoints 
            };
            
            var discountResult = _discountCalculator.CalculateTotalDiscount(discountContext);
            string notes = discountResult.Note;

            decimal subtotalAfterDiscount = baseAmount - discountResult.Amount;
            if (subtotalAfterDiscount < 300m)
            {
                subtotalAfterDiscount = 300m;
                notes += "minimum discounted subtotal applied; ";
            }

            decimal supportFee = _feeCalculatorService.CalculateSupportFee(normalizedPlanCode, includePremiumSupport);
            if (includePremiumSupport) notes += "premium support included; ";

            decimal paymentFee = _feeCalculatorService.CalculatePaymentFee(normalizedPaymentMethod, subtotalAfterDiscount + supportFee);
            notes += GetPaymentMethodNote(normalizedPaymentMethod);

            decimal taxBase = subtotalAfterDiscount + supportFee + paymentFee;
            decimal taxRate = _taxCalculatorService.GetTaxRate(customer.Country);
            decimal taxAmount = taxBase * taxRate;
            
            decimal finalAmount = taxBase + taxAmount;
            if (finalAmount < 500m)
            {
                finalAmount = 500m;
                notes += "minimum invoice amount applied; ";
            }

            var invoice = new RenewalInvoice
            {
                InvoiceNumber = $"INV-{DateTime.UtcNow:yyyyMMdd}-{customerId}-{normalizedPlanCode}",
                CustomerName = customer.FullName,
                PlanCode = normalizedPlanCode,
                PaymentMethod = normalizedPaymentMethod,
                SeatCount = seatCount,
                BaseAmount = Math.Round(baseAmount, 2, MidpointRounding.AwayFromZero),
                DiscountAmount = Math.Round(baseAmount - subtotalAfterDiscount, 2, MidpointRounding.AwayFromZero),
                SupportFee = Math.Round(supportFee, 2, MidpointRounding.AwayFromZero),
                PaymentFee = Math.Round(paymentFee, 2, MidpointRounding.AwayFromZero),
                TaxAmount = Math.Round(taxAmount, 2, MidpointRounding.AwayFromZero),
                FinalAmount = Math.Round(finalAmount, 2, MidpointRounding.AwayFromZero),
                Notes = notes.Trim(),
                GeneratedAt = DateTime.UtcNow
            };

            _billingGateway.SaveInvoice(invoice);

            if (!string.IsNullOrWhiteSpace(customer.Email))
            {
                string subject = "Subscription renewal invoice";
                string body = $"Hello {customer.FullName}, your renewal for plan {normalizedPlanCode} has been prepared. Final amount: {invoice.FinalAmount:F2}.";
                _billingGateway.SendEmail(customer.Email, subject, body);
            }

            return invoice;
        }

        private string GetPaymentMethodNote(string paymentMethod)
        {
            return paymentMethod switch
            {
                "CARD" => "card payment fee; ",
                "BANK_TRANSFER" => "bank transfer fee; ",
                "PAYPAL" => "paypal fee; ",
                "INVOICE" => "invoice payment; ",
                _ => string.Empty
            };
        }
    }
}