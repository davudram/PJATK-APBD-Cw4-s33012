using LegacyRenewalApp.Interfaces;

namespace LegacyRenewalApp.Strategies.PaymentFees;

public class CardPaymentFeeStrategy : IPaymentFeeStrategy
{
    public bool Supports(string paymentMethod) => paymentMethod == "CARD";
    public decimal CalculateFee(decimal amountToCharge) => amountToCharge * 0.02m;
}