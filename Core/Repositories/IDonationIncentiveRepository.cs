using Core.Entities;

namespace Core.Repositories;

public interface IDonationIncentiveRepository {
  public IDonationIncentive Get();
  public IDonationIncentive Set(IDonationIncentive donationIncentive);

  public double GetGoal();
  public double SetGoal(double goal);
  public double AddToGoal(double amount);
  public double RemoveFromGoal(double amount);

  public double GetAmount();
  public double SetAmount(double amount);
  public double AddAmount(double amount);
  public double RemoveAmount(double amount);

  public void Initialize(IDonationIncentive incentive);
}