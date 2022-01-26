using Core.Twitch;
using Core.Twitch.DTOs;

namespace Infrastructure.Twitch;

public class TwitchAPI : ITwitchApi {
  public IObservable<ChannelCheerDto> ChannelCheer() {
    throw new NotImplementedException();
  }

  public IObservable<ChannelSubscriptionDto> ChannelSubscription() {
    throw new NotImplementedException();
  }
}