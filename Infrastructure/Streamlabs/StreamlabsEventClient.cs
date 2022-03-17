using System.Reactive.Linq;
using System.Reactive.Subjects;
using Core.Interfaces;
using Core.Streamlabs;
using Core.Streamlabs.Events;
using Infrastructure.Shared.Typing;
using Infrastructure.Streamlabs.Socket.Dto;
using Newtonsoft.Json;

namespace Infrastructure.Streamlabs;

public class StreamlabsEventClient : IStreamlabsEventClient, IDisposable {
  public StreamlabsEventClient(IStreamlabsSocketClient socketClient, IEventDtoContainer container) {
    SocketClient = socketClient;
    Container = container;
  }

  public IStreamlabsSocketClient SocketClient { get; }
  internal IDisposable _websocketSubscription { get; set; }
  internal IEventDtoContainer Container { get; }

  public void Dispose() {
    SocketClient.Dispose();
    _websocketSubscription?.Dispose();
  }

  public ISubject<object> EventReceived { get; } = new Subject<object>();

  public bool IsSubscribed { get; private set; }

  public async Task SubscribeToEvents() {
    if (IsSubscribed) {
      return;
    }

    await SocketClient.Connect();
    _websocketSubscription = SocketClient.OnEvent().Subscribe(OnEvent);
    IsSubscribed = true;
  }

  public async Task UnsubscribeFromEvents() {
    _websocketSubscription.Dispose();
    await SocketClient.Disconnect();
  }

  public IObservable<IDonation> DonationReceived() {
    return EventReceived.OfType<IDonation>();
  }

  public IObservable<IStreamlabsDonation> StreamlabsDonationReceived() {
    return EventReceived.OfType<IStreamlabsDonation>();
  }

  public IObservable<ITwitchSubscription> TwitchSubscriptionReceived() {
    return EventReceived.OfType<ITwitchSubscription>();
  }

  public IObservable<ITwitchBitsCheer> TwitchBitsCheerReceived() {
    return EventReceived.OfType<ITwitchBitsCheer>();
  }

  public void OnEvent(string eventData) {
    var dtoType = GetDtoTypeFromEvent(eventData);

    if (dtoType is null) {
      return;
    }

    Console.WriteLine(eventData);

    var genericEventType = typeof(BaseWebsocketEvent<>).MakeGenericType(dtoType);
    var dto = JsonConvert.DeserializeObject(eventData, genericEventType);
    var data = GetEventDataFromDto(dto);

    EventReceived.OnNext(data);

    if (data is IDonation donation) {
      EventReceived.OnNext(donation);
    }
  }

  private Type? GetDtoTypeFromEvent(string eventData) {
    var baseEvent = JsonConvert.DeserializeObject<BaseWebsocketEvent<object>>(eventData)!;
    var eventType = baseEvent.Type;
    var type = Container.GetDtoFromEventName(eventType);

    return type?.GetType();
  }

  private object GetEventDataFromDto(dynamic dto) {
    return dto.Message[0];
  }
}