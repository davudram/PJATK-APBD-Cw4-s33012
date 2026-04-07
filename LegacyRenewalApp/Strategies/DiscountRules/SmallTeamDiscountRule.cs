using LegacyRenewalApp.Interfaces;

namespace LegacyRenewalApp.Strategies.DiscountRules;

public class SmallTeamDiscountRule : IDiscountRule
{
    public bool IsApplicable(DiscountContext context) => context.SeatCount >= 10 && context.SeatCount < 20;

    public DiscountResult Calculate(DiscountContext context) => 
        new DiscountResult { Amount = context.BaseAmount * 0.04m, Note = "small team discount; " };
}