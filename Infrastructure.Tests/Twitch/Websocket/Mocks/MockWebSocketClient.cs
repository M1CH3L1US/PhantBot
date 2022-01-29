using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Websocket.Client;
using Websocket.Client.Models;

namespace Infrastructure.Test.Twitch.Websocket.Mocks;

public class MockWebSocketClient : IWebsocketClient {
  public MockWebSocketClient() {
    MessageReceivedSubject = new Subject<ResponseMessage>();
    ReconnectionHappenedSubject = new Subject<ReconnectionInfo>();
    DisconnectionHappenedSubject = new Subject<DisconnectionInfo>();
  }

  private Subject<ReconnectionInfo> ReconnectionHappenedSubject { get; }
  private Subject<DisconnectionInfo> DisconnectionHappenedSubject { get; }
  private Subject<ResponseMessage> MessageReceivedSubject { get; }

  public List<object> MessagesSent { get; } = new();

  public string Name { get; set; } = "MockWebSocketClient";
  public Uri? Url { get; set; }

  public IObservable<ResponseMessage> MessageReceived => MessageReceivedSubject;
  public IObservable<ReconnectionInfo> ReconnectionHappened => ReconnectionHappenedSubject;
  public IObservable<DisconnectionInfo> DisconnectionHappened => DisconnectionHappenedSubject;

  public TimeSpan? ReconnectTimeout { get; set; }
  public TimeSpan? ErrorReconnectTimeout { get; set; }
  public bool IsStarted { get; private set; }
  public bool IsRunning { get; private set; }
  public bool IsReconnectionEnabled { get; set; }
  public bool IsTextMessageConversionEnabled { get; set; }
  public Encoding? MessageEncoding { get; set; }
  public ClientWebSocket? NativeClient { get; } = null;

  public void StreamFakeMessage(ResponseMessage message) {
    MessageReceivedSubject.OnNext(message);
  }

  public void Dispose() {
    IsStarted = false;
    IsRunning = false;
  }

  public async Task Start() {
    IsStarted = true;
    IsRunning = true;
  }

  public async Task StartOrFail() {
    await Start();
  }

  public async Task<bool> Stop(WebSocketCloseStatus status, string statusDescription) {
    IsStarted = false;
    IsRunning = false;
    return true;
  }

  public Task<bool> StopOrFail(WebSocketCloseStatus status, string statusDescription) {
    return Stop(status, statusDescription);
  }

  public void Send(string message) {
    MessageSent(message);
  }

  public void Send(byte[] message) {
    MessageSent(message);
  }

  public void Send(ArraySegment<byte> message) {
    MessageSent(message);
  }

  public async Task SendInstant(string message) {
    MessageSent(message);
  }

  public async Task SendInstant(byte[] message) {
    MessageSent(message);
  }

  public Task Reconnect() {
    return Task.Delay(10);
  }

  public Task ReconnectOrFail() {
    return Reconnect();
  }

  private void MessageSent(object message) {
    MessagesSent.Add(message);
  }

  public void SendFakeMessage(object message) {
    var jsonMessage = JsonConvert.SerializeObject(message);
    var r = ResponseMessage.TextMessage(jsonMessage);
    MessageReceivedSubject.OnNext(r);
  }
}