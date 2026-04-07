using LegacyRenewalApp.Interfaces;

namespace LegacyRenewalApp.Strategies.DiscountRules;

public class MediumTeamDiscountRule : IDiscountRule
{
    public bool IsApplicable(DiscountContext context) => context.SeatCount >= 20 && context.SeatCount < 50;

    public DiscountResult Calculate(DiscountContext context) => 
        new DiscountResult { Amount = context.BaseAmount * 0.08m, Note = "medium team discount; " };
}