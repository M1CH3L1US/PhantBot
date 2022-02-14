using Core.Repositories;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Xunit;

namespace Infrastructure.Tests.Repositories;

public class DonationIncentiveMemoryRepositoryTest {
  private readonly IDonationIncentiveRepository _sut = new DonationIncentiveMemoryRepository();

  [Fact]
  public async void Get_EmptyIncentive_WhenNoIncentiveExists() {
    var incentive = await _sut.Get();

    Assert.Equal(0, incentive.Amount);
  }

  [Fact]
  public async void Get_IncentiveKeepsValue_WhenIncentiveExists() {
    var incentive = await _sut.Get();

    incentive.Amount = 100;
    await _sut.Set(incentive);
    var updatedIncentive = await _sut.Get();

    Assert.Equal(100, updatedIncentive.Amount);
  }

  [Theory]
  [InlineData(50)]
  public async void AddAmount_IncentiveAmountIsIncreased_WhenAmountIsAdded(decimal amount) {
    await _sut.AddAmount(amount);
    var updatedAmount = await _sut.GetAmount();

    Assert.Equal(50, updatedAmount);
  }


  [Theory]
  [InlineData(50, 25)]
  public async void RemoveAmount_IncentiveAmountIsReduced_WhenAmountIsRemoved(decimal initialAmount,
    decimal amountToRemove) {
    var incentive = new DonationIncentive {Amount = initialAmount};

    await _sut.Initialize(incentive);
    await _sut.RemoveAmount(amountToRemove);

    var expectedAmount = initialAmount - amountToRemove;
    var updatedAmount = await _sut.GetAmount();

    Assert.Equal(expectedAmount, updatedAmount);
  }

  [Theory]
  [InlineData(50)]
  public async void AddGoal_IncentiveGoalIsIncreased_WhenAmountIsAdded(decimal amount) {
    await _sut.AddToGoal(amount);
    var updatedAmount = await _sut.GetGoal();

    Assert.Equal(amount, updatedAmount);
  }


  [Theory]
  [InlineData(50, 25)]
  public async void RemoveGoal_IncentiveGoalIsReduced_WhenAmountIsRemoved(decimal initialGoal, decimal amountToRemove) {
    var incentive = new DonationIncentive {Goal = initialGoal};

    await _sut.Initialize(incentive);
    await _sut.RemoveFromGoal(amountToRemove);

    var expectedAmount = initialGoal - amountToRemove;
    var updatedAmount = await _sut.GetGoal();

    Assert.Equal(expectedAmount, updatedAmount);
  }
}