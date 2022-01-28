using Newtonsoft.Json;

namespace Infrastructure.Twitch.Dto.Websocket;

public class EmoteDto {
  [JsonProperty("start")] public double Start { get; set; }

  [JsonProperty("end")] public double End { get; set; }

  [JsonProperty("id")] public double Id { get; set; }
}