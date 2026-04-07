using LegacyRenewalApp.Interfaces;

namespace LegacyRenewalApp.Strategies.DiscountRules;

public class EducationDiscountRule : IDiscountRule
{
    public bool IsApplicable(DiscountContext context) => 
        context.Customer.Segment == "Education" && context.Plan.IsEducationEligible;

    public DiscountResult Calculate(DiscountContext context) => 
        new DiscountResult { Amount = context.BaseAmount * 0.20m, Note = "education discount; " };
}