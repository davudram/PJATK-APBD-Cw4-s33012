using LegacyRenewalApp.Interfaces;

namespace LegacyRenewalApp.Repositories;

public class DiscountRuleRepository: IDiscountRuleRepository
{
    public DiscountResult Calculate(DiscountContext context)
    {
        var segment = context.Customer.Segment;
        decimal baseAmount = context.BaseAmount;

        if (segment == "Silver") return new DiscountResult { Amount = baseAmount * 0.05m, Note = "silver discount; " };
        if (segment == "Gold") return new DiscountResult { Amount = baseAmount * 0.10m, Note = "gold discount; " };
        if (segment == "Platinum") return new DiscountResult { Amount = baseAmount * 0.15m, Note = "platinum discount; " };
        if (segment == "Education" && context.Plan.IsEducationEligible) return new DiscountResult { Amount = baseAmount * 0.20m, Note = "education discount; " };

        return new DiscountResult { Amount = 0, Note = string.Empty };
    }
}