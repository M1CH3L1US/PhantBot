using System;
using System.Threading.Tasks;
using Core.Configuration;
using Core.Streamlabs;
using FluentAssertions;
using Infrastructure.Streamlabs.Websocket;
using Infrastructure.Streamlabs.Websocket.Dto;
using Infrastructure.Tests.Utils;
using Newtonsoft.Json;
using Xunit;

namespace Infrastructure.Tests.Streamlabs.Websocket;

public class StreamlabsWebsocketClientTest {
  private readonly IStreamlabsWebsocketClient _sut;
  private readonly MockWebSocketClient _websocketClient;

  public StreamlabsWebsocketClientTest(
    IApplicationConfiguration configuration,
    IStreamlabsAuthClient authClient,
    MockWebSocketClient websocketClient
  ) {
    _websocketClient = websocketClient;
    _sut = new StreamlabsWebsocketClient(websocketClient, authClient, configuration);
  }

  [Fact]
  public async Task Connect_ShouldConnectWebsocket_WhenCalled() {
    await _sut.Connect();

    _sut.IsConnected.Should().BeTrue();
  }

  [Fact]
  public async Task Connect_ShouldConnectWebsocket_WhenCalledTwice() {
    await _sut.Connect();
    await _sut.Connect();

    _sut.IsConnected.Should().BeTrue();
  }

  [Theory]
  [JsonFile("Streamlabs/Websocket/TestData/donation.json", typeof(BaseWebsocketEvent<StreamlabsDonation>))]
  [JsonFile("Streamlabs/Websocket/TestData/twitch-subscription.json", typeof(BaseWebsocketEvent<TwitchSubscription>))]
  [JsonFile("Streamlabs/Websocket/TestData/twitch-bits.json", typeof(BaseWebsocketEvent<TwitchBitsCheer>))]
  public async Task OnEvent_ShouldEmitEvent_WhenEventIsReceived(object data) {
    await _sut.Connect();
    string? received = null;
    var expected = JsonConvert.SerializeObject(data);

    _sut.OnEvent().Subscribe(e => received = e);
    _websocketClient.ReceiveFakeMessage(data);

    received.Should().Be(expected);
  }
}