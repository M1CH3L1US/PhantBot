using Core.Twitch.Http;
using Infrastructure.Twitch.Dto.Websocket;

namespace Infrastructure.Twitch;

public class TwitchApiClient : ITwitchApiClient {
  public IObservable<ChannelSubscriptionDto> ChannelSubscription() {
    throw new NotImplementedException();
  }

  public IObservable<ChannelCheerDto> ChannelCheer() {
    throw new NotImplementedException();
  }
}