using Core.Streamlabs.Events;
using Infrastructure.Shared.Typing;
using Newtonsoft.Json;

namespace Infrastructure.Streamlabs.Websocket.Dto;

public class StreamlabsDonation : IStreamlabsDonation, IEventDto {
  private string _formattedAmount;

  public string EventName { get; } = "donation";

  [JsonProperty("name")]
  string IStreamlabsDonation.Name { get; set; }

  [JsonProperty("message")]
  string IStreamlabsDonation.Message { get; set; }

  [JsonProperty("from_user_id")]
  string? IStreamlabsDonation.UserId { get; set; }

  [JsonProperty("currency")]
  string IStreamlabsDonation.Currency { get; set; }

  [JsonProperty("amount")]
  decimal IStreamlabsDonation.Amount { get; set; }

  [JsonProperty("formattedAmount")]
  string IStreamlabsDonation.FormattedAmount {
    get => _formattedAmount;
    set => _formattedAmount = value;
  }

  [JsonProperty("from")]
  string IStreamlabsDonation.FromName { get; set; }


  public string GetValueString() {
    return _formattedAmount;
  }
}