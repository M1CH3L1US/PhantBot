using Core.Twitch.DTOs;

namespace Core.Twitch;

public interface ITwitchApi {
  IObservable<ChannelSubscriptionDto> ChannelSubscription();
  IObservable<ChannelCheerDto> ChannelCheer();
}