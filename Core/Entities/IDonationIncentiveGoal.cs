namespace Core.Entities;

public interface IDonationIncentive : ICloneable {
  public string Name { get; set; }
  public decimal Goal { get; set; }
  public decimal Amount { get; set; }

  public decimal PercentageCompleted { get; }
}