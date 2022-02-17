namespace Core.Entities;

public interface IDonationIncentive : ICloneable {
  public decimal Goal { get; set; }
  public decimal Amount { get; set; }

  public decimal GetPercentage() {
    if (Goal <= 0) {
      return 0;
    }

    return Amount / Goal * 100;
  }
}