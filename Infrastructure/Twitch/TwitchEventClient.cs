using Core.Twitch.Events;
using Core.Twitch.Websocket;

namespace Infrastructure.Twitch;

public class TwitchEventClient : ITwitchEventClient {
  public TwitchEventClient(ITwitchWebsocketClient websocketClient) {
    WebsocketClient = websocketClient;
  }

  private ITwitchWebsocketClient WebsocketClient { get; }

  public IObservable<IChannelCheerEvent> OnChannelCheer(string channelId) {
    var topic = ListenTopic.BitsEvent(channelId);
    return WebsocketClient.OnMessage<IChannelCheerEvent>(topic);
  }

  public IObservable<IChannelSubscriptionEvent> OnChannelSubscribe(string channelId) {
    var topic = ListenTopic.ChannelSubscription(channelId);
    return WebsocketClient.OnMessage<IChannelSubscriptionEvent>(topic);
  }

  public IObservable<string> OnChannelMessage() {
    throw new NotImplementedException();
  }
}