using System;
using System.Collections.Generic;
using System.Linq;
using LegacyRenewalApp.Interfaces;

namespace LegacyRenewalApp.Services;

public class FeeCalculatorService
{
    private readonly IEnumerable<IPaymentFeeStrategy> _paymentStrategies;

    public FeeCalculatorService(IEnumerable<IPaymentFeeStrategy> paymentStrategies)
    {
        _paymentStrategies = paymentStrategies;
    }

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
        var strategy = _paymentStrategies.FirstOrDefault(s => s.Supports(paymentMethod));
        
        if (strategy == null)
        {
            throw new ArgumentException($"Unsupported payment method: {paymentMethod}");
        }

        return strategy.CalculateFee(amountToCharge);
    }
}