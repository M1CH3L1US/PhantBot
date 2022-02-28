using System;
using System.Threading.Tasks;
using Core.Configuration;
using Core.Streamlabs;
using FluentAssertions;
using Infrastructure.Streamlabs.Socket;
using Infrastructure.Streamlabs.Socket.Dto;
using Infrastructure.Tests.Utils;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Socket.Io.Client.Core;
using Xunit;

namespace Infrastructure.Tests.Streamlabs.Websocket;

public class StreamlabsWebsocketClientTest {
  private readonly ISocketIoClient _socketIoClient;
  private readonly IStreamlabsSocketClient _sut;

  public StreamlabsWebsocketClientTest(
    IOptions<StreamlabsConfiguration> configuration,
    IStreamlabsAuthClient authClient,
    ISocketIoClient socketIoClient
  ) {
    _socketIoClient = socketIoClient;
    _sut = new StreamlabsSocketClient(socketIoClient, authClient, configuration);
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
    _socketIoClient.Emit("event", expected);

    received.Should().Be(expected);
  }
}