using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using Socket.Io.Client.Core;
using Socket.Io.Client.Core.Model;
using Socket.Io.Client.Core.Model.SocketEvent;

[assembly: IgnoresAccessChecksTo("Socket.Io.Client.Core")]

namespace Infrastructure.Tests.Mocking;

public class MockSocketIoClient : ISocketIoClient {
  private readonly ISubject<EventMessageEvent> _events = new Subject<EventMessageEvent>();

  public void Dispose() {
    IsRunning = false;
  }

  public async Task OpenAsync(Uri uri, SocketIoOpenOptions options = null) {
    IsRunning = true;
  }

  public async Task CloseAsync() {
    IsRunning = false;
  }

  public IObservable<AckMessageEvent> Emit(string eventName) {
    _events.OnNext(MakeMessageEvent(eventName));
    return Observable.Empty<AckMessageEvent>();
  }

  public IObservable<AckMessageEvent> Emit<TData>(string eventName, TData data) {
    _events.OnNext(MakeMessageEvent(eventName, data));
    return Observable.Empty<AckMessageEvent>();
  }

  public void EmitAcknowledge<TData>(int packetId, TData data) {
  }

  public IObservable<EventMessageEvent> On(string eventName) {
    return _events.Where(e => e.EventName == eventName);
  }

  public SocketIoEvents Events { get; }
  public bool IsRunning { get; private set; }
  public SocketIoClientOptions Options { get; }

  private EventMessageEvent MakeMessageEvent(string eventName, object? data = null) {
    var element = JsonSerializer.SerializeToElement(data);
    var list = new List<JsonElement> {
      element
    };

    return new EventMessageEvent(eventName, true, o => { }, list);
  }
}