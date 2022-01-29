using System;
using System.Linq;
using Core.Configuration;
using Core.Twitch.Websocket;
using Infrastructure.Configuration;
using Infrastructure.Test.Configuration;
using Infrastructure.Test.Twitch.Websocket.Mocks;
using Infrastructure.Twitch.Websocket;
using Newtonsoft.Json;
using Xunit;

namespace Infrastructure.Test.Twitch.Websocket;

public class TwitchWebsocketClientTest {
  private readonly IApplicationConfiguration _configuration;
  private readonly Payloads _paylods;
  private readonly ITwitchWebsocketClient _sut;
  private readonly MockWebSocketClient _websocketClient;

  public TwitchWebsocketClientTest() {
    _websocketClient = new MockWebSocketClient();
    _configuration = new ApplicationConfiguration(MockApplicationConfiguration.Create());
    _sut = new TwitchWebsocketClient(_websocketClient, _configuration);
    _paylods = new Payloads(_configuration);
  }

  [Fact]
  public void Connect_ShouldConnectOnTheWebsocketClient() {
    _sut.Connect();

    Assert.True(_websocketClient.IsRunning);

    Teardown();
  }

  [Fact]
  public void Connect_ShouldRegisterPingInterval_AfterConnected() {
    _sut.Connect();

    var isPinging = (_sut as TwitchWebsocketClient)?.PingInterval is not null;

    Assert.True(isPinging);

    Teardown();
  }

  [Fact]
  public void Connect_ShouldUnregisterPingInterval_WhenDisconnected() {
    _sut.Connect();
    _sut.Disconnect();

    var isNotPinging = (_sut as TwitchWebsocketClient)?.PingInterval is null;

    Assert.True(isNotPinging);

    Teardown();
  }


  [Fact]
  public async void Ping_ShouldSendPayloadToWebsocket_WhenCalled() {
    await _sut.Connect();
    await _sut.Ping();

    var payloadSent = (string) _websocketClient.MessagesSent.First();
    var expectedPayload = JsonConvert.SerializeObject(_paylods.PingRequest);

    Assert.Equal(expectedPayload, payloadSent);

    Teardown();
  }

  [Fact]
  public async void Ping_ShouldReceiveValidResponseFromWebsocketServer_AfterBeingCalled() {
    await _sut.Connect();
    await _sut.Ping();

    string? response = null;
    _sut.OnMessage().Subscribe(res => response = res);
    _websocketClient.SendFakeMessage(_paylods.PingResponse);
    var expectedResponse = JsonConvert.SerializeObject(_paylods.PingResponse);

    Assert.Equal(response, expectedResponse);

    Teardown();
  }

  [Fact]
  public async void ListenToTopics_ShouldSendListenPayload_WhenTopicIsAdded() {
    await _sut.Connect();
    await _sut.Ping();

    var channel = "TestChannel";
    var topics = new[] {ListenTopic.BitsEvent(channel), ListenTopic.ChannelSubscription(channel)};

    await _sut.ListenToTopics(topics);

    var payload = _paylods.ListenRequest(topics);
    var expectedPayload = JsonConvert.SerializeObject(payload);
    var hasPayload = _websocketClient.MessagesSent.Contains(expectedPayload);

    Assert.True(hasPayload);

    Teardown();
  }

  [Fact]
  public async void OnMessage_ShouldEmitMessagePayloadString_WhenMessageIsReceived() {
    await _sut.Connect();
    await _sut.Ping();

    string? response = null;
    _sut.OnMessage().Subscribe(res => response = res);
    _websocketClient.SendFakeMessage(_paylods.BitsMessage);
    var expectedResponse = JsonConvert.SerializeObject(_paylods.BitsMessage);

    Assert.Equal(response, expectedResponse);

    Teardown();
  }

  private void Teardown() {
    _websocketClient.MessagesSent.Clear();
    _sut.Disconnect();
  }
}