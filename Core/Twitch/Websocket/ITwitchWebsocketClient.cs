namespace Core.Twitch.Websocket;

public interface ITwitchWebsocketClient {
  public bool IsConnected { get; }

  public Task Connect();
  public Task Reconnect();
  public Task Disconnect();

  public Task SendMessage(object message);
  public Task Ping();

  public Task ListenToTopic(ListenTopic topic);
  public Task ListenToTopics(IEnumerable<ListenTopic> topics);

  public IObservable<string> OnMessage();
}