using LegacyRenewalApp.Interfaces;

namespace LegacyRenewalApp.Strategies.PaymentFees;

public class PayPalPaymentFeeStrategy : IPaymentFeeStrategy
{
    public bool Supports(string paymentMethod) => paymentMethod == "PAYPAL";
    public decimal CalculateFee(decimal amountToCharge) => amountToCharge * 0.035m;
}