using Xunit;

namespace Infrastructure.Test.Twitch.MapExtensions;

public class ChannelCheerEventExtensionTest {
  [Fact]
  public void ToChannelCheerEvent_CorrectlyCastsDto_WhenUserIsAnonymous() {
    //var cheerEvent = _anonymousChannelCheer.ToEvent<IChannelCheerEvent>();

    //Assert.Equal(cheerEvent.Username, null);
    //Assert.Equal(_anonymousChannelCheer.bits, cheerEvent.Bits);
  }


  [Fact]
  public void ToChannelCheerEvent_CorrectlyCastsDto_WhenUserIsNotAnonymous() {
    // var cheerEvent = _channelCheer.ToEvent<IChannelCheerEvent>();
    // 
    // Assert.Equal(_channelCheer.user_name, cheerEvent.Username);
    // Assert.Equal(_channelCheer.bits, cheerEvent.Bits);
  }
}