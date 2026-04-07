using System.Collections.Generic;
using System.Linq;
using LegacyRenewalApp.Interfaces;

namespace LegacyRenewalApp;

public class DiscountService : IDiscountCalculator
{
    private readonly IEnumerable<IDiscountRule> _rules;

    public DiscountService(IEnumerable<IDiscountRule> rules)
    {
        _rules = rules;
    }

    public DiscountResult CalculateTotalDiscount(DiscountContext context)
    {
        var applicableRules = _rules.Where(r => r.IsApplicable(context)).ToList();

        if (applicableRules.Any())
        {
            return applicableRules
                .Select(r => r.Calculate(context))
                .OrderByDescending(res => res.Amount)
                .First();
        }

        return new DiscountResult { Amount = 0, Note = string.Empty };
    }
}