using System.Reactive.Linq;
using System.Reactive.Subjects;
using Core.Interfaces;
using Core.Streamlabs;
using Core.Streamlabs.Events;
using Infrastructure.Shared.Typing;
using Infrastructure.Streamlabs.Websocket.Dto;
using Newtonsoft.Json;

namespace Infrastructure.Streamlabs;

public class StreamlabsEventClient : IStreamlabsEventClient, IDisposable {
  public StreamlabsEventClient(IStreamlabsWebsocketClient websocketClient, IEventDtoContainer container) {
    WebsocketClient = websocketClient;
    Container = container;
  }

  public IStreamlabsWebsocketClient WebsocketClient { get; }
  internal IDisposable _websocketSubscription { get; set; }
  internal IEventDtoContainer Container { get; }

  public void Dispose() {
    WebsocketClient.Dispose();
    _websocketSubscription.Dispose();
  }

  public ISubject<object> EventReceived { get; } = new Subject<object>();

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

    var genericEventType = typeof(BaseWebsocketEvent<>).MakeGenericType(dtoType);
    var dto = JsonConvert.DeserializeObject(eventData, genericEventType) as BaseWebsocketEvent<object>;
    var data = GetEventDataFromDto(dto!);

    EventReceived.OnNext((IDonation) data);
  }

  private Type? GetDtoTypeFromEvent(string eventData) {
    var baseEvent = JsonConvert.DeserializeObject<BaseWebsocketEvent<object>>(eventData)!;
    var eventType = baseEvent.Type;
    var type = Container.GetDtoFromEventName(eventType);

    return type?.GetType();
  }

  private T GetEventDataFromDto<T>(BaseWebsocketEvent<T> dto) {
    return dto.Message[0];
  }
}