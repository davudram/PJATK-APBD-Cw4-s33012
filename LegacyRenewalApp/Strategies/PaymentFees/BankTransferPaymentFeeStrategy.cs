using LegacyRenewalApp.Interfaces;

namespace LegacyRenewalApp.Strategies.PaymentFees;

public class BankTransferPaymentFeeStrategy : IPaymentFeeStrategy
{
    public bool Supports(string paymentMethod) => paymentMethod == "BANK_TRANSFER";
    public decimal CalculateFee(decimal amountToCharge) => amountToCharge * 0.01m;
}