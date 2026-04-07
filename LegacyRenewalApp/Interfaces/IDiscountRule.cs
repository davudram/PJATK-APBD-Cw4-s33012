namespace LegacyRenewalApp.Interfaces;

public interface IDiscountRule
{
    bool IsApplicable(DiscountContext context);
    DiscountResult Calculate(DiscountContext context);
}