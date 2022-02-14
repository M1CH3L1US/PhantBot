using Core.Entities;
using Core.Repositories;
using Infrastructure.Entities;

namespace Infrastructure.Repositories;

public class DonationIncentiveMemoryRepository : IDonationIncentiveRepository {
  private IDonationIncentive? _incentive;

  private IDonationIncentive Incentive {
    get {
      _incentive ??= new DonationIncentive();
      return _incentive;
    }
    set => _incentive = value;
  }

  public async Task<IDonationIncentive> Get() {
    return Incentive;
  }

  public async Task<IDonationIncentive> Set(IDonationIncentive incentive) {
    var previousIncentive = incentive;
    Incentive = incentive;
    return previousIncentive;
  }

  public async Task<decimal> SetGoal(decimal goal) {
    var previousGoal = GetGoal();
    Incentive.Goal = goal;
    return await previousGoal;
  }

  public async Task<decimal> SetAmount(decimal amount) {
    var previousAmount = GetAmount();
    Incentive.Amount = amount;
    return await previousAmount;
  }

  public async Task Initialize(IDonationIncentive incentive) {
    Incentive = incentive;
  }

  public async Task<decimal> GetGoal() {
    return Incentive.Goal;
  }

  public async Task<decimal> AddToGoal(decimal amount) {
    return Incentive.Goal += amount;
  }

  public async Task<decimal> RemoveFromGoal(decimal amount) {
    return Incentive.Goal -= amount;
  }

  public async Task<decimal> GetAmount() {
    return Incentive.Amount;
  }

  public async Task<decimal> AddAmount(decimal amount) {
    return Incentive.Amount += amount;
  }

  public async Task<decimal> RemoveAmount(decimal amount) {
    return Incentive.Amount -= amount;
  }
}