using Core.Repositories;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Xunit;

namespace Infrastructure.Tests.Repositories;

public class DonationIncentiveMemoryRepositoryTest {
  private readonly IDonationIncentiveRepository _sut = new DonationIncentiveMemoryRepository();

  [Fact]
  public void Get_EmptyIncentive_WhenNoIncentiveExists() {
    var incentive = _sut.Get();

    Assert.Equal(0, incentive.Amount);
  }

  [Fact]
  public void Get_IncentiveKeepsValue_WhenIncentiveExists() {
    var incentive = _sut.Get();

    incentive.Amount = 100;
    _sut.Set(incentive);
    var updatedIncentive = _sut.Get();

    Assert.Equal(100, updatedIncentive.Amount);
  }

  [Theory]
  [InlineData(50)]
  public void AddAmount_IncentiveAmountIsIncreased_WhenAmountIsAdded(double amount) {
    _sut.AddAmount(amount);
    var updatedAmount = _sut.GetAmount();

    Assert.Equal(50, updatedAmount);
  }


  [Theory]
  [InlineData(50, 25)]
  public void RemoveAmount_IncentiveAmountIsReduced_WhenAmountIsRemoved(double initialAmount, double amountToRemove) {
    var incentive = new DonationIncentive {Amount = initialAmount};

    _sut.Initialize(incentive);
    _sut.RemoveAmount(amountToRemove);

    var expectedAmount = initialAmount - amountToRemove;
    var updatedAmount = _sut.GetAmount();

    Assert.Equal(expectedAmount, updatedAmount);
  }

  [Theory]
  [InlineData(50)]
  public void AddGoal_IncentiveGoalIsIncreased_WhenAmountIsAdded(double amount) {
    _sut.AddToGoal(amount);
    var updatedAmount = _sut.GetGoal();

    Assert.Equal(amount, updatedAmount);
  }


  [Theory]
  [InlineData(50, 25)]
  public void RemoveGoal_IncentiveGoalIsReduced_WhenAmountIsRemoved(double initialGoal, double amountToRemove) {
    var incentive = new DonationIncentive {Goal = initialGoal};

    _sut.Initialize(incentive);
    _sut.RemoveFromGoal(amountToRemove);

    var expectedAmount = initialGoal - amountToRemove;
    var updatedAmount = _sut.GetGoal();

    Assert.Equal(expectedAmount, updatedAmount);
  }
}