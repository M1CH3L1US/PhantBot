using Newtonsoft.Json;

namespace Infrastructure.Streamlabs.Websocket.Dto;

public class StreamlabsSocketToken {
  [JsonProperty("socket_token")]
  public string Token { get; set; }
}