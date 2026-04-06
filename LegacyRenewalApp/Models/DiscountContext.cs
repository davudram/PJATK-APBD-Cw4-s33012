namespace LegacyRenewalApp;

public class DiscountContext
{
    public Customer Customer { get; set; } = null!;
    public SubscriptionPlan Plan { get; set; } = null!;
    public int SeatCount { get; set; }
    public decimal BaseAmount { get; set; }
    public bool UseLoyaltyPoints { get; set; }
}