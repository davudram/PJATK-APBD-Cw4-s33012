using System.Collections.Generic;
using LegacyRenewalApp.Interfaces;

namespace LegacyRenewalApp;

public class DiscountCalculatorRepository : IDiscountCalculator
{
    private readonly IEnumerable<IDiscountRuleRepository> _rules;

    public DiscountCalculatorRepository(IEnumerable<IDiscountRuleRepository> rules)
    {
        _rules = rules;
    }

    public DiscountResult CalculateTotalDiscount(DiscountContext context)
    {
        decimal totalAmount = 0m;
        string combinedNotes = string.Empty;

        foreach (var rule in _rules)
        {
            var result = rule.Calculate(context);
            totalAmount += result.Amount;
            combinedNotes += result.Note;
        }

        if (context.UseLoyaltyPoints && context.Customer.LoyaltyPoints > 0)
        {
            int pointsToUse = context.Customer.LoyaltyPoints > 200 ? 200 : context.Customer.LoyaltyPoints;
            totalAmount += pointsToUse;
            combinedNotes += $"loyalty points used: {pointsToUse}; ";
        }

        return new DiscountResult { Amount = totalAmount, Note = combinedNotes };
    }
}