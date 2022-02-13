using System;
using Core.Finance;
using Core.Interfaces;
using Core.Streamlabs;
using Core.Streamlabs.Events;
using FluentAssertions;
using Infrastructure.Shared.Typing;
using Infrastructure.Streamlabs;
using Infrastructure.Tests.Utils;
using Xunit;

namespace Infrastructure.Tests.Streamlabs;

public class StreamlabsEventClientTest {
  private readonly IDonationConverter _converter;
  private readonly StreamlabsEventClient _sut;

  public StreamlabsEventClientTest(IEventDtoContainer container, IStreamlabsWebsocketClient client,
    IDonationConverter converter) {
    _sut = new StreamlabsEventClient(client, container);
    _converter = converter;
  }

  [Fact]
  public async void SubscribeToEvents_OpensWebsocketConnection_WhenCalled() {
    await _sut.SubscribeToEvents();
    _sut.WebsocketClient.IsConnected.Should().BeTrue();
  }

  [Fact]
  public async void UnsubscribeFromEvents_ClosesWebsocketConnection_WhenCalled() {
    await _sut.SubscribeToEvents();
    await _sut.UnsubscribeFromEvents();

    _sut.WebsocketClient.IsConnected.Should().BeFalse();
  }

  [Theory]
  [FileContent("Streamlabs/Websocket/TestData/donation.json")]
  [FileContent("Streamlabs/Websocket/TestData/twitch-subscription.json")]
  [FileContent("Streamlabs/Websocket/TestData/twitch-bits.json")]
  public async void OnEvent_IsCalledWithEventData_WhenEventIsReceived(string data) {
    object? eventData = null;
    await _sut.SubscribeToEvents();

    _sut.EventReceived.Subscribe(e => eventData = e);
    _sut.WebsocketClient.WebsocketEventReceived.OnNext(data);

    Assert.NotNull(eventData);
  }

  [Theory]
  [FileContent("Streamlabs/Websocket/TestData/donation.json")]
  public async void OnStreamlabsDonationReceived_EmitsCastedEvent_WhenEventIsReceived(string donation) {
    object? eventData = null;
    await _sut.SubscribeToEvents();

    _sut.EventReceived.Subscribe(e => eventData = e);
    _sut.WebsocketClient.WebsocketEventReceived.OnNext(donation);

    eventData.Should().BeAssignableTo<IStreamlabsDonation>();
  }

  [Theory]
  [FileContent("Streamlabs/Websocket/TestData/donation.json")]
  [FileContent("Streamlabs/Websocket/TestData/twitch-subscription.json")]
  [FileContent("Streamlabs/Websocket/TestData/twitch-bits.json")]
  public async void OnDonationReceived_EmitsIDonationEvent_WhenEventIsReceived(string donation) {
    IDonation? eventData = null;
    await _sut.SubscribeToEvents();

    _sut.DonationReceived().Subscribe(e => eventData = e);
    _sut.WebsocketClient.WebsocketEventReceived.OnNext(donation);
    var valueInDollars = await eventData!.ConvertToCurrency(_converter);

    eventData.Should().BeAssignableTo<IDonation>();
    valueInDollars.Should().BeApproximately(1m, decimal.One);
  }
}