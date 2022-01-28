using Core.Twitch.Events;

namespace Core.Twitch.Websocket;

public interface ITwitchEventClient {
  IObservable<IChannelCheerEvent> OnChannelCheer(string channelId);
  IObservable<IChannelSubscriptionEvent> OnChannelSubscribe(string channelId);
  IObservable<string> OnChannelMessage();
}