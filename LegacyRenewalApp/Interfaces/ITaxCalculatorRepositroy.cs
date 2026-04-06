namespace LegacyRenewalApp.Interfaces;

public interface ITaxCalculatorRepositroy
{
    decimal GetTaxRate(string country);
}