using LegacyRenewalApp.Interfaces;

namespace LegacyRenewalApp.Strategies.DiscountRules;

public class GoldDiscountRule : IDiscountRule
{
    public bool IsApplicable(DiscountContext context) => context.Customer.Segment == "Gold";

    public DiscountResult Calculate(DiscountContext context) => 
        new DiscountResult { Amount = context.BaseAmount * 0.10m, Note = "gold discount; " };
}