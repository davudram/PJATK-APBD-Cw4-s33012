using LegacyRenewalApp.Interfaces;

namespace LegacyRenewalApp.Strategies.DiscountRules;

public class LargeTeamDiscountRule : IDiscountRule
{
    public bool IsApplicable(DiscountContext context) => context.SeatCount >= 50;

    public DiscountResult Calculate(DiscountContext context) => 
        new DiscountResult { Amount = context.BaseAmount * 0.12m, Note = "large team discount; " };
}