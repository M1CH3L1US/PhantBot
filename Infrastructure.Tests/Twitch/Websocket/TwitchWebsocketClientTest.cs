using System.Linq;
using Core.Twitch.Websocket;
using Infrastructure.Test.Twitch.Websocket.Mocks;
using Infrastructure.Twitch.Websocket;
using Newtonsoft.Json;
using Xunit;

namespace Infrastructure.Test.Twitch.Websocket;

public class TwitchWebsocketClientTest {
  private static readonly object PingPayload = WebsocketRequestBuilder.BuildRequest(RequestType.Ping);
  private readonly ITwitchWebsocketClient _sut;
  private readonly MockWebSocketClient _websocketClient;

  public TwitchWebsocketClientTest() {
    _websocketClient = new MockWebSocketClient();
    _sut = new TwitchWebsocketClient(_websocketClient);
  }

  [Fact]
  public void Connect_ShouldConnectOnTheWebsocketClient() {
    _sut.Connect();

    Assert.True(_websocketClient.IsRunning);

    _sut.Disconnect();
  }

  [Fact]
  public void Connect_ShouldRegisterPingInterval_AfterConnected() {
    _sut.Connect();

    var isPinging = (_sut as TwitchWebsocketClient)?.PingInterval is not null;

    Assert.True(isPinging);

    _sut.Disconnect();
  }

  [Fact]
  public void Connect_ShouldUnregisterPingInterval_WhenDisconnected() {
    _sut.Connect();
    _sut.Disconnect();

    var isNotPinging = (_sut as TwitchWebsocketClient)?.PingInterval is null;

    Assert.True(isNotPinging);

    _sut.Disconnect();
  }


  [Fact]
  public async void Ping_ShouldSendPayloadToWebsocket_WhenCalled() {
    await _sut.Connect();
    await _sut.Ping();

    var payloadSent = (string) _websocketClient.MessagesSent.First();
    var expectedPayload = JsonConvert.SerializeObject(PingPayload);

    Assert.Equal(expectedPayload, payloadSent);

    await _sut.Disconnect();
  }

  [Fact]
  public void Ping_ShouldGetValidResponseFromWebsocketServer_AfterBeingCalled() {
    _sut.Connect();
  }

  [Fact]
  public void ListenToTopics_ShouldSendListenPayload_WhenTopicIsAdded() {
    _sut.Connect();
  }
}