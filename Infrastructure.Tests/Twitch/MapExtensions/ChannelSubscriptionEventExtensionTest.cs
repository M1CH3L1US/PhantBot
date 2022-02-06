using Xunit;

namespace Infrastructure.Tests.Twitch.MapExtensions;

public class ChannelSubscriptionEventExtensionTest {
  [Fact]
  public void ToChannelCheerEvent_CorrectlyCastsDto_WhenUserIsAnonymous() {
    //  var channelSubscriptionEvent = _channelSubscription.ToEvent<IChannelSubscriptionEvent>();
    // 
    //  Assert.Equal(channelSubscriptionEvent.Tier, ChannelSubscriptionTier.TierOne);
    //  Assert.Equal(_channelSubscription.user_name, channelSubscriptionEvent.Username);
  }
}