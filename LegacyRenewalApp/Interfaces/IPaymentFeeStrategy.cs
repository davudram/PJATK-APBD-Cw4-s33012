namespace LegacyRenewalApp.Interfaces;

public interface IPaymentFeeStrategy
{
    bool Supports(string paymentMethod);
    decimal CalculateFee(decimal amountToCharge);
}