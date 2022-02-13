using Core.Streamlabs.Events;

namespace Core.Finance;

public interface IDonationConverter {
  public Task<decimal> ConvertFromTwitchSubscription(ITwitchSubscription subscription);
  public Task<decimal> ConvertFromStreamlabsDonation(IStreamlabsDonation donation);
  public Task<decimal> ConvertFromBits(ITwitchBitsCheer cheer);
}