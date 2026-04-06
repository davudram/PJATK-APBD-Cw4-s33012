namespace LegacyRenewalApp.Interfaces;

public interface IDiscountCalculator
{
    DiscountResult CalculateTotalDiscount(DiscountContext context);
}