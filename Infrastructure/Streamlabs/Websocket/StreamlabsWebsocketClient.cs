using System.Net.WebSockets;
using System.Reactive.Linq;
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

  public bool IsConnected => WebsocketClient.IsRunning;

  public async Task Connect() {
    if (IsConnected) {
      return;
    }

    await InitializeClient();
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

    return WebsocketClient.MessageReceived
                          .Select(message => message.Text);
  }

  private async Task InitializeClient() {
    var websocketUrl = await GetConnectionUri();
    WebsocketClient.Url = new Uri(websocketUrl);
  }

  private async Task<string> GetConnectionUri() {
    var authToken = await AuthClient.GetSocketAuthToken();
    var websocketUri = Configuration.WebsocketUri;
    return $"{websocketUri}?token={authToken}";
  }
}