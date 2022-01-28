using System;
using System.Net.WebSockets;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Websocket.Client;
using Websocket.Client.Models;

namespace Infrastructure.Test.Twitch.Websocket.Mocks;

public class MockWebSocketClient : IWebsocketClient {
  public MockWebSocketClient(Subject<DisconnectionInfo> disconnectionHappenedSubject) {
    MessageReceivedSubject = new Subject<ResponseMessage>();
    ReconnectionHappenedSubject = new Subject<ReconnectionInfo>();
    DisconnectionHappenedSubject = new Subject<DisconnectionInfo>();
  }

  private Subject<ReconnectionInfo> ReconnectionHappenedSubject { get; }
  private Subject<DisconnectionInfo> DisconnectionHappenedSubject { get; }
  private Subject<ResponseMessage> MessageReceivedSubject { get; }

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
  public Encoding MessageEncoding { get; set; }
  public ClientWebSocket? NativeClient { get; } = null;

  public void StreamFakeMessage(ResponseMessage message) {
    MessageReceivedSubject.OnNext(message);
  }

  public void Dispose() {
  }

  public async Task Start() {
    IsStarted = true;
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
  }

  public void Send(byte[] message) {
  }

  public void Send(ArraySegment<byte> message) {
  }

  public async Task SendInstant(string message) {
  }

  public async Task SendInstant(byte[] message) {
  }

  public Task Reconnect() {
    return Task.Delay(10);
  }

  public Task ReconnectOrFail() {
    return Reconnect();
  }

  public void SendFakeMessage(object message) {
    var jsonMessage = JsonConvert.SerializeObject(message);
    var r = ResponseMessage.TextMessage(jsonMessage);
    MessageReceivedSubject.OnNext(r);
  }
}