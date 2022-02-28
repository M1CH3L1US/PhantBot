using Newtonsoft.Json;

namespace Infrastructure.Streamlabs.Socket.Dto;

public class StreamlabsSocketToken {
  [JsonProperty("socket_token")]
  public string Token { get; set; }
}