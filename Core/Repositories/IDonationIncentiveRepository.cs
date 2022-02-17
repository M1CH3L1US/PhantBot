using Core.Entities;

namespace Core.Repositories;

public interface IDonationIncentiveRepository {
  public Task<IDonationIncentive> GetOrCreate();
  public Task<IDonationIncentive> Set(IDonationIncentive donationIncentive);

  public Task<decimal> GetGoal();
  public Task<IDonationIncentive> SetGoal(decimal goal);
  public Task<IDonationIncentive> AddToGoal(decimal amount);
  public Task<IDonationIncentive> RemoveFromGoal(decimal amount);

  public Task<decimal> GetAmount();
  public Task<IDonationIncentive> SetAmount(decimal amount);
  public Task<IDonationIncentive> AddToAmount(decimal amount);
  public Task<IDonationIncentive> RemoveFromAmount(decimal amount);
}