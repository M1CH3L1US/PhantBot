using Core.Twitch.DTOs;
using Core.Twitch.Events;
using Core.Twitch.MapExtensions;
using Xunit;

namespace Core.Test.MapExtensions;

public class ChannelCheerEventExtensionTest {
  private readonly ChannelCheerDto _anonymousChannelCheer = new() {
    bits = 1000,
    user_id = null,
    user_name = null,
    user_login = null,
    broadcaster_user_id = "12345",
    broadcaster_user_name = "TestBroadcaster",
    broadcaster_user_login = "TestBroadcasterLogin",
    is_anonymous = true,
    message = "TestMessage"
  };

  private readonly ChannelCheerDto _channelCheer = new() {
    bits = 1000,
    user_id = "67890",
    user_name = "TestUser",
    user_login = "TestLogin",
    broadcaster_user_id = "12345",
    broadcaster_user_name = "TestBroadcaster",
    broadcaster_user_login = "TestBroadcasterLogin",
    is_anonymous = false,
    message = "TestMessage"
  };

  [Fact]
  public void ToChannelCheerEvent_CorrectlyCastsDto_WhenUserIsAnonymous() {
    var cheerEvent = _anonymousChannelCheer.ToEvent<IChannelCheerEvent>();

    Assert.Equal(cheerEvent.Username, null);
    Assert.Equal(_anonymousChannelCheer.bits, cheerEvent.Bits);
  }


  [Fact]
  public void ToChannelCheerEvent_CorrectlyCastsDto_WhenUserIsNotAnonymous() {
    var cheerEvent = _channelCheer.ToEvent<IChannelCheerEvent>();

    Assert.Equal(_channelCheer.user_name, cheerEvent.Username);
    Assert.Equal(_channelCheer.bits, cheerEvent.Bits);
  }
}