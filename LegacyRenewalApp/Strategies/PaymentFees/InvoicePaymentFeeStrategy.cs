using LegacyRenewalApp.Interfaces;

namespace LegacyRenewalApp.Strategies.PaymentFees;

public class InvoicePaymentFeeStrategy : IPaymentFeeStrategy
{
    public bool Supports(string paymentMethod) => paymentMethod == "INVOICE";
    public decimal CalculateFee(decimal amountToCharge) => 0m;
}