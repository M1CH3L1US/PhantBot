using Core.Entities;
using Infrastructure.Entities;
using Xunit;

namespace Infrastructure.Tests.Entities;

public class DonationIncentiveTest {
  [Fact]
  public void GetPercentage_IsZero_ByDefault() {
    IDonationIncentive incentive = new DonationIncentive();
    var percentage = incentive.GetPercentage();

    Assert.Equal(0, percentage);
  }


  [Fact]
  public void GetPercentage_CalculatesPercentage_IfGoalAndAmountAreSet() {
    IDonationIncentive incentive = new DonationIncentive {Amount = 20, Goal = 100};
    var percentage = incentive.GetPercentage();

    Assert.Equal(20, percentage);
  }
}