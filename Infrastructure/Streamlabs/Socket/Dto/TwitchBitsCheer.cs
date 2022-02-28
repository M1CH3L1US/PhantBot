using Core.Finance;
using Core.Streamlabs.Events;
using Infrastructure.Shared.Typing;
using Newtonsoft.Json;

namespace Infrastructure.Streamlabs.Socket.Dto;

public class TwitchBitsCheer : ITwitchBitsCheer, IEventDto {
  public string EventName { get; } = "bits";

  [JsonProperty("message")]
  string ITwitchBitsCheer.Message { get; set; }

  [JsonProperty("name")]
  string ITwitchBitsCheer.Name { get; set; }

  [JsonProperty("amount")]
  int ITwitchBitsCheer.Amount { get; set; }

  public Task<decimal> ConvertToCurrency(IDonationConverter converter) {
    return converter.ConvertFromBits(this);
  }
}