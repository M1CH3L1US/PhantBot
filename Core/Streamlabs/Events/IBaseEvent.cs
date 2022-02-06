namespace Core.Streamlabs.Events;

public interface IBaseWebsocketEvent<T> {
  public string Type { get; set; }
  public List<T> Message { get; set; }
}