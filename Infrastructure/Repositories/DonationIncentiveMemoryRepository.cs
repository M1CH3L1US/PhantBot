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

  public double GetGoal() {
    return Incentive.Goal;
  }

  public double AddToGoal(double amount) {
    return Incentive.Goal += amount;
  }

  public double RemoveFromGoal(double amount) {
    return Incentive.Goal -= amount;
  }

  public double GetAmount() {
    return Incentive.Amount;
  }

  public double AddAmount(double amount) {
    return Incentive.Amount += amount;
  }

  public double RemoveAmount(double amount) {
    return Incentive.Amount -= amount;
  }

  public IDonationIncentive Get() {
    return Incentive;
  }

  public IDonationIncentive Set(IDonationIncentive incentive) {
    var previousIncentive = incentive;
    Incentive = incentive;
    return previousIncentive;
  }

  public double SetGoal(double goal) {
    var previousGoal = GetGoal();
    Incentive.Goal = goal;
    return previousGoal;
  }

  public double SetAmount(double amount) {
    var previousAmount = GetAmount();
    Incentive.Amount = amount;
    return previousAmount;
  }

  public void Initialize(IDonationIncentive incentive) {
    Incentive = incentive;
  }
}