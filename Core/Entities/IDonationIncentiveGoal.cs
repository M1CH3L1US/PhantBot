namespace Core.Entities;

public interface IDonationIncentive {
  public double Goal { get; set; }
  public double Amount { get; set; }

  public double GetPercentage() {
    if (Goal <= 0) return 0;

    return Amount / Goal * 100;
  }
}