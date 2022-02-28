using Core.Streamlabs.Events;
using Infrastructure.Shared.Typing;
using Newtonsoft.Json;

namespace Infrastructure.Streamlabs.Socket.Dto;

public class TwitchFollow : ITwitchFollow, IEventDto {
  public string EventName { get; } = "follow";

  [JsonProperty("name")]
  public string Name { get; set; }
}