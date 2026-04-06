using System;
using LegacyRenewalApp.Interfaces;

namespace LegacyRenewalApp.Repositories;

public class FeeCalculatorRepositoryRepository : IFeeCalculatorRepository
{
    public decimal CalculateSupportFee(string planCode, bool includePremiumSupport)
    {
        if (!includePremiumSupport) return 0m;

        return planCode switch
        {
            "START" => 250m,
            "PRO" => 400m,
            "ENTERPRISE" => 700m,
            _ => 0m
        };
    }

    public decimal CalculatePaymentFee(string paymentMethod, decimal amountToCharge)
    {
        return paymentMethod switch
        {
            "CARD" => amountToCharge * 0.02m,
            "BANK_TRANSFER" => amountToCharge * 0.01m,
            "PAYPAL" => amountToCharge * 0.035m,
            "INVOICE" => 0m,
            _ => throw new ArgumentException("Unsupported payment method")
        };
    }
}