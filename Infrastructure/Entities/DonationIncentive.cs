using Core.Entities;

namespace Infrastructure.Entities;

public class DonationIncentive : IDonationIncentive {
  public double Goal { get; set; } = 0;

  public double Amount { get; set; } = 0;
}