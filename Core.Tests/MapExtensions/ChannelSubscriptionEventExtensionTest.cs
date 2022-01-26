using Core.Twitch;
using Core.Twitch.DTOs;
using Core.Twitch.Events;
using Core.Twitch.MapExtensions;
using Xunit;

namespace Core.Test.MapExtensions;

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