using LegacyRenewalApp.Interfaces;

namespace LegacyRenewalApp.Strategies.DiscountRules;

public class PlatinumDiscountRule : IDiscountRule
{
    public bool IsApplicable(DiscountContext context) => context.Customer.Segment == "Platinum";

    public DiscountResult Calculate(DiscountContext context) => 
        new DiscountResult { Amount = context.BaseAmount * 0.15m, Note = "platinum discount; " };
}