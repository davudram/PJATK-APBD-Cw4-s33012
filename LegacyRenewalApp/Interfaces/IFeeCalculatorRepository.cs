namespace LegacyRenewalApp.Interfaces;

public interface IFeeCalculatorRepository
{
    decimal CalculateSupportFee(string planCode, bool includePremiumSupport);
    decimal CalculatePaymentFee(string paymentMethod, decimal amountToCharge);
}