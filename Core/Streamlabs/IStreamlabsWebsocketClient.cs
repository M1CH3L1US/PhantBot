namespace Core.Streamlabs;

public interface IStreamlabsWebsocketClient : IDisposable {
  public bool IsConnected { get; }

  public Task Connect();
  public Task Disconnect();

  public IObservable<string> OnEvent();
}