using LegacyRenewalApp.Interfaces;

namespace LegacyRenewalApp.Repositories;

public class TeamSizeDiscountRuleRepository: IDiscountRuleRepository
{
    public DiscountResult Calculate(DiscountContext context)
    {
        decimal baseAmount = context.BaseAmount;
        if (context.SeatCount >= 50) return new DiscountResult { Amount = baseAmount * 0.12m, Note = "large team discount; " };
        if (context.SeatCount >= 20) return new DiscountResult { Amount = baseAmount * 0.08m, Note = "medium team discount; " };
        if (context.SeatCount >= 10) return new DiscountResult { Amount = baseAmount * 0.04m, Note = "small team discount; " };
            
        return new DiscountResult { Amount = 0, Note = string.Empty };
    }
}