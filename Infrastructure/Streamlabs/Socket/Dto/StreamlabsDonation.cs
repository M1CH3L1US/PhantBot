using Core.Finance;
using Core.Streamlabs.Events;
using Infrastructure.Shared.Typing;
using Newtonsoft.Json;

namespace Infrastructure.Streamlabs.Socket.Dto;

public class StreamlabsDonation : IStreamlabsDonation, IEventDto {
  private decimal _amount { get; set; }
  private string _currency { get; set; }
  public string EventName { get; } = "donation";

  [JsonProperty("name")]
  string IStreamlabsDonation.Name { get; set; }

  [JsonProperty("message")]
  string IStreamlabsDonation.Message { get; set; }

  [JsonProperty("from_user_id")]
  string? IStreamlabsDonation.UserId { get; set; }

  [JsonProperty("currency")]
  string IStreamlabsDonation.Currency {
    get => _currency;
    set => _currency = value;
  }

  [JsonProperty("amount")]
  decimal IStreamlabsDonation.Amount {
    get => _amount;
    set => _amount = value;
  }

  [JsonProperty("formattedAmount")]
  string IStreamlabsDonation.FormattedAmount { get; set; }

  [JsonProperty("from")]
  string IStreamlabsDonation.FromName { get; set; }

  public Task<decimal> ConvertToCurrency(IDonationConverter converter) {
    return converter.ConvertFromStreamlabsDonation(this);
  }
}