using Core.Twitch.Events;

namespace Core.Twitch;

public interface ITwitchEventClient {
  IObservable<IChannelCheerEvent> OnChannelCheer();
  IObservable<IChannelSubscriptionEvent> OnChannelSubscribe();
}