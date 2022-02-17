using Core.Entities;
using Newtonsoft.Json;

namespace Infrastructure.Entities;

public class DonationIncentive : IDonationIncentive {
  [JsonProperty("goal")]
  public decimal Goal { get; set; }

  [JsonProperty("amount")]
  public decimal Amount { get; set; }

  public object Clone() {
    return new DonationIncentive {
      Goal = Goal, Amount = Amount
    };
  }
}