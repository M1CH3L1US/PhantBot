using System.Reactive.Linq;
using System.Reactive.Subjects;
using Core.Configuration;
using Core.Streamlabs;
using Microsoft.Extensions.Options;
using Socket.Io.Client.Core;

namespace Infrastructure.Streamlabs.Socket;

public class StreamlabsSocketClient : IStreamlabsSocketClient {
  private Uri _connectionUrl;

  public StreamlabsSocketClient(
    ISocketIoClient socketClient,
    IStreamlabsAuthClient authClient,
    IOptions<StreamlabsConfiguration> configuration
  ) {
    SocketClient = socketClient;
    AuthClient = authClient;
    Configuration = configuration.Value;
  }

  internal ISocketIoClient SocketClient { get; }
  internal StreamlabsConfiguration Configuration { get; }
  internal IStreamlabsAuthClient AuthClient { get; }

  public ISubject<string> SocketEventReceived { get; } = new Subject<string>();

  public bool IsConnected => SocketClient.IsRunning;

  public async Task Connect() {
    if (IsConnected) {
      return;
    }

    await InitializeClient();
    SubscribeToEvents();
    await ConnectWebsocketClient();
  }

  public Task Disconnect() {
    if (!IsConnected) {
      return Task.CompletedTask;
    }

    return SocketClient.CloseAsync();
  }

  public void Dispose() {
    Disconnect();
    SocketClient.Dispose();
  }

  public IObservable<string> OnEvent() {
    if (!IsConnected) {
      throw new InvalidOperationException("Not connected");
    }

    return SocketEventReceived;
  }

  private async Task InitializeClient() {
    var websocketUrl = await GetConnectionUri();
    _connectionUrl = new Uri(websocketUrl);
  }

  private Task ConnectWebsocketClient() {
    return SocketClient.OpenAsync(_connectionUrl);
  }

  private void SubscribeToEvents() {
    SocketClient.On("event")
                .Select(e => e.Data[0].ToString())
                .Subscribe(d => SocketEventReceived.OnNext(d));
  }

  private async Task<string> GetConnectionUri() {
    var authToken = await AuthClient.GetSocketAuthToken();
    var websocketUri = Configuration.WebsocketUri;
    return $"{websocketUri}?token={authToken}";
  }
}