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

  public StreamlabsEventClientTest(
    IEventDtoContainer container,
    IStreamlabsSocketClient client,
    IDonationConverter converter
  ) {
    _sut = new StreamlabsEventClient(client, container);
    _converter = converter;
  }

  [Fact]
  public async void SubscribeToEvents_OpensWebsocketConnection_WhenCalled() {
    await _sut.SubscribeToEvents();
    _sut.SocketClient.IsConnected.Should().BeTrue();
  }

  [Fact]
  public async void UnsubscribeFromEvents_ClosesWebsocketConnection_WhenCalled() {
    await _sut.SubscribeToEvents();
    await _sut.UnsubscribeFromEvents();

    _sut.SocketClient.IsConnected.Should().BeFalse();
  }

  [Theory]
  [FileContent("Streamlabs/Websocket/TestData/donation.json")]
  [FileContent("Streamlabs/Websocket/TestData/twitch-subscription.json")]
  [FileContent("Streamlabs/Websocket/TestData/twitch-bits.json")]
  public async void OnEvent_IsCalledWithEventData_WhenEventIsReceived(string data) {
    object? eventData = null;
    await _sut.SubscribeToEvents();

    _sut.EventReceived.Subscribe(e => eventData = e);
    _sut.SocketClient.SocketEventReceived.OnNext(data);

    Assert.NotNull(eventData);
  }

  [Theory]
  [FileContent("Streamlabs/Websocket/TestData/donation.json")]
  public async void OnStreamlabsDonationReceived_EmitsCastedEvent_WhenEventIsReceived(string donation) {
    object? eventData = null;
    await _sut.SubscribeToEvents();

    _sut.EventReceived.Subscribe(e => eventData = e);
    _sut.SocketClient.SocketEventReceived.OnNext(donation);

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
    _sut.SocketClient.SocketEventReceived.OnNext(donation);
    var valueInDollars = await eventData!.ConvertToCurrency(_converter);

    eventData.Should().BeAssignableTo<IDonation>();
    valueInDollars.Should().BeApproximately(1m, decimal.One);
  }

  [Fact]
  public async void OnDonationReceived_EmitsIDonationEvent_WhenEvenMultipleEventsAreReceived() {
    var slDonationEvent = FileHelper.GetFileContent("Streamlabs/Websocket/TestData/donation.json");
    var twitchSubscriptionEvent = FileHelper.GetFileContent("Streamlabs/Websocket/TestData/twitch-subscription.json");
    var twitchBitsEvent = FileHelper.GetFileContent("Streamlabs/Websocket/TestData/twitch-bits.json");

    IDonation? eventData = null;
    await _sut.SubscribeToEvents();

    _sut.DonationReceived().Subscribe(e => eventData = e);
    _sut.SocketClient.SocketEventReceived.OnNext(slDonationEvent);
    eventData.Should().BeAssignableTo<IStreamlabsDonation>();
    _sut.SocketClient.SocketEventReceived.OnNext(twitchSubscriptionEvent);
    eventData.Should().BeAssignableTo<ITwitchSubscription>();
    _sut.SocketClient.SocketEventReceived.OnNext(twitchBitsEvent);
    eventData.Should().BeAssignableTo<ITwitchBitsCheer>();
    _sut.SocketClient.SocketEventReceived.OnNext(twitchSubscriptionEvent);
    eventData.Should().BeAssignableTo<ITwitchSubscription>();
  }

  [Theory]
  [FileContent("Streamlabs/Websocket/TestData/twitch-follow.json")]
  public async void OnDonationReceived_DoesNotEmitEvent_WhenFollowIsReceived(string follow) {
    IDonation? eventData = null;
    await _sut.SubscribeToEvents();

    _sut.DonationReceived().Subscribe(e => eventData = e);
    _sut.SocketClient.SocketEventReceived.OnNext(follow);

    eventData.Should().BeNull();
  }
}