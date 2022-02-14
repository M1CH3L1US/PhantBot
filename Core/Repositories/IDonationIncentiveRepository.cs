using Core.Entities;

namespace Core.Repositories;

public interface IDonationIncentiveRepository {
  public Task<IDonationIncentive> Get();
  public Task<IDonationIncentive> Set(IDonationIncentive donationIncentive);

  public Task<decimal> GetGoal();
  public Task<decimal> SetGoal(decimal goal);
  public Task<decimal> AddToGoal(decimal amount);
  public Task<decimal> RemoveFromGoal(decimal amount);

  public Task<decimal> GetAmount();
  public Task<decimal> SetAmount(decimal amount);
  public Task<decimal> AddAmount(decimal amount);
  public Task<decimal> RemoveAmount(decimal amount);

  public Task Initialize(IDonationIncentive incentive);
}