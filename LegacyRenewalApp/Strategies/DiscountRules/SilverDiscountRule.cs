using LegacyRenewalApp.Interfaces;

namespace LegacyRenewalApp.Strategies.DiscountRules;

public class SilverDiscountRule : IDiscountRule
{
    public bool IsApplicable(DiscountContext context) => context.Customer.Segment == "Silver";

    public DiscountResult Calculate(DiscountContext context) => 
        new DiscountResult { Amount = context.BaseAmount * 0.05m, Note = "silver discount; " };
}