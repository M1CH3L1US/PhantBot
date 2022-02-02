using Newtonsoft.Json;

namespace Infrastructure.Streamlabs.Websocket.Dto;

public interface IBaseEvent {
  [JsonProperty("type")]
  public string Type { get; set; }

  [JsonProperty("message")]
  public object[] Message { get; set; }
}