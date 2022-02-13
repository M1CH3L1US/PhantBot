using System.Net.WebSockets;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Core.Configuration;
using Core.Streamlabs;
using Websocket.Client;

namespace Infrastructure.Streamlabs.Websocket;

public class StreamlabsWebsocketClient : IStreamlabsWebsocketClient {
  public StreamlabsWebsocketClient(
    IWebsocketClient websocketClient,
    IStreamlabsAuthClient authClient,
    IApplicationConfiguration configuration
  ) {
    WebsocketClient = websocketClient;
    AuthClient = authClient;
    Configuration = configuration.Streamlabs;
  }

  internal IWebsocketClient WebsocketClient { get; }
  internal IStreamlabsConfiguration Configuration { get; }
  internal IStreamlabsAuthClient AuthClient { get; }

  public ISubject<string> WebsocketEventReceived { get; } = new Subject<string>();

  public bool IsConnected => WebsocketClient.IsRunning;

  public async Task Connect() {
    if (IsConnected) {
      return;
    }

    await InitializeClient();
    SubscribeToEvents();
    await WebsocketClient.Start();
  }

  public Task Disconnect() {
    if (!IsConnected) {
      return Task.CompletedTask;
    }

    return WebsocketClient.Stop(WebSocketCloseStatus.NormalClosure, "Disconnecting");
  }

  public void Dispose() {
    Disconnect();
  }

  public IObservable<string> OnEvent() {
    if (!IsConnected) {
      throw new InvalidOperationException("Not connected");
    }

    return WebsocketEventReceived;
  }

  private async Task InitializeClient() {
    var websocketUrl = await GetConnectionUri();
    WebsocketClient.Url = new Uri(websocketUrl);
  }

  private void SubscribeToEvents() {
    WebsocketClient.MessageReceived.Select(message => message.Text)
                   .Subscribe(WebsocketEventReceived);
  }

  private async Task<string> GetConnectionUri() {
    var authToken = await AuthClient.GetSocketAuthToken();
    var websocketUri = Configuration.WebsocketUri;
    return $"{websocketUri}?token={authToken}";
  }
}