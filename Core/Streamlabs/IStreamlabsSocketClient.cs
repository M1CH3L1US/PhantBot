using System.Reactive.Subjects;

namespace Core.Streamlabs;

public interface IStreamlabsSocketClient : IDisposable {
  public bool IsConnected { get; }
  public ISubject<string> SocketEventReceived { get; }

  public Task Connect();
  public Task Disconnect();
  public IObservable<string> OnEvent();
}