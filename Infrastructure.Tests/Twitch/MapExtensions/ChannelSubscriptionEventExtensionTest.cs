using Core.Twitch.Enums;
using Core.Twitch.Events;
using Infrastructure.Twitch.DTOs;
using Infrastructure.Twitch.MapExtensions;
using Xunit;

namespace Infrastructure.Test.Twitch.MapExtensions;

public class ChannelSubscriptionEventExtensionTest {
  private readonly ChannelSubscriptionDto _channelSubscription = new() {
    tier = "1000",
    broadcaster_user_id = "123",
    user_id = "456",
    user_name = "test",
    broadcaster_user_login = "test",
    broadcaster_user_name = "test",
    is_gift = false,
    user_login = "test"
  };

  [Fact]
  public void ToChannelCheerEvent_CorrectlyCastsDto_WhenUserIsAnonymous() {
    var channelSubscriptionEvent = _channelSubscription.ToEvent<IChannelSubscriptionEvent>();

    Assert.Equal(channelSubscriptionEvent.Tier, ChannelSubscriptionTier.TierOne);
    Assert.Equal(_channelSubscription.user_name, channelSubscriptionEvent.Username);
  }
}