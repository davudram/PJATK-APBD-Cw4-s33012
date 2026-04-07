using System;
using System.Collections.Generic;

namespace LegacyRenewalApp.Services;

public class TaxCalculatorService
{
    private readonly Dictionary<string, decimal> _taxRates = new(StringComparer.OrdinalIgnoreCase)
    {
        { "Poland", 0.23m },
        { "Germany", 0.19m },
        { "Czech Republic", 0.21m },
        { "Norway", 0.25m }
    };

    public decimal GetTaxRate(string country)
    {
        return _taxRates.TryGetValue(country, out var rate) ? rate : 0.20m;
    }
}