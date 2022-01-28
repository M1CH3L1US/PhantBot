using System.Net.WebSockets;
using System.Reactive.Linq;
using Core.Twitch.Websocket;
using Newtonsoft.Json;
using Websocket.Client;

namespace Infrastructure.Twitch.Websocket;

public class TwitchWebsocketClient : ITwitchWebsocketClient {
  private readonly IWebsocketClient _ws;
  internal IDisposable? PingInterval;

  public TwitchWebsocketClient(IWebsocketClient ws) {
    _ws = ws;
  }

  public bool IsConnected => _ws.IsRunning;

  public Task Connect() {
    if (IsConnected) throw new InvalidOperationException("Already connected");

    RegisterPingInterval();

    return _ws.Start();
  }

  public Task Disconnect() {
    UnregisterPingInterval();
    return _ws.Stop(WebSocketCloseStatus.NormalClosure, "");
  }

  public async Task Reconnect() {
    if (!IsConnected) return;

    UnregisterPingInterval();
    await _ws.ReconnectOrFail();
    RegisterPingInterval();
  }

  public Task Ping() {
    var pingRequest = WebsocketRequestBuilder.BuildRequest(RequestType.Ping);
    return SendMessage(pingRequest);
  }

  public Task ListenToTopic(ListenTopic topic) {
    return ListenToTopics(new[] {topic});
  }

  public Task ListenToTopics(IEnumerable<ListenTopic> topics) {
    var topicString = topics.Select(listenTopic => listenTopic.Topic);
    var data = new {
      topics = topicString.ToArray(),
      auth_token = "__"
    };
    var request = WebsocketRequestBuilder.BuildRequest(RequestType.Listen, data);

    return SendMessage(request);
  }

  public Task SendMessage(object message) {
    var jsonMessage = JsonConvert.SerializeObject(message);
    return _ws.SendInstant(jsonMessage);
  }

  public IObservable<T> OnMessage<T>(ListenTopic topic) where T : class {
    if (!IsConnected) throw new InvalidOperationException("Not connected");

    return _ws.MessageReceived
              .Select(m => m.Text)
              .Select(JsonConvert.DeserializeObject<T>)!;
  }

  ~TwitchWebsocketClient() {
    _ws.Dispose();
  }

  /// The Twitch API requires us to send a PING
  /// message every 5 minutes.
  private void RegisterPingInterval() {
    var interval = Observable.Interval(TimeSpan.FromMinutes(4.5));
    PingInterval = interval.Subscribe(_ => { Ping(); });
  }

  private void UnregisterPingInterval() {
    PingInterval?.Dispose();
    PingInterval = null;
  }
}