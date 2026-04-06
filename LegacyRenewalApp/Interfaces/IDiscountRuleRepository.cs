namespace LegacyRenewalApp.Interfaces;

public interface IDiscountRuleRepository
{
    DiscountResult Calculate(DiscountContext context);
}