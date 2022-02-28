using Core.Streamlabs.Events;
using Newtonsoft.Json;

namespace Infrastructure.Streamlabs.Socket.Dto;

public class BaseWebsocketEvent<T> : IBaseWebsocketEvent<T> {
  [JsonProperty("message")]
  public List<T> Message { get; set; }

  [JsonProperty("type")]
  public string Type { get; set; }
}