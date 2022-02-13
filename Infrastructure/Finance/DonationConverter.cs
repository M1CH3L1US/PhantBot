using Core.Enums;
using Core.Finance;
using Core.Streamlabs.Events;

namespace Infrastructure.Finance;

public class DonationConverter : IDonationConverter {
  private const decimal TwitchBitValue = 0.01m;
  private readonly ICurrencyConverter _converter;

  private readonly Dictionary<ChannelSubscriptionTier, decimal> _twitchSubscriptionValue = new() {
    {ChannelSubscriptionTier.TierOne, 2.5m},
    {ChannelSubscriptionTier.TierTwo, 1m},
    {ChannelSubscriptionTier.TierThree, 1m}
  };

  public DonationConverter(ICurrencyConverter converter, ICurrency targetCurrency) {
    TargetCurrency = targetCurrency;
    _converter = converter;
  }

  public ICurrency TargetCurrency { get; set; }

  public async Task<decimal> ConvertFromTwitchSubscription(ITwitchSubscription subscription) {
    var tier = subscription.SubscriptionTier;
    return _twitchSubscriptionValue[tier];
  }

  public Task<decimal> ConvertFromStreamlabsDonation(IStreamlabsDonation donation) {
    var amount = donation.Amount;
    var donationCurrency = Currency.FromCode(donation.Currency);

    if (donationCurrency is null) {
      donationCurrency = Currency.Usd;
    }

    return _converter.Convert(amount, donationCurrency, TargetCurrency);
  }

  public async Task<decimal> ConvertFromBits(ITwitchBitsCheer cheer) {
    var amountOfBits = cheer.Amount;
    return amountOfBits * TwitchBitValue;
  }
}