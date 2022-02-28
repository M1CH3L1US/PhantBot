using Core.Entities;
using Newtonsoft.Json;

namespace Infrastructure.Entities;

public class DonationIncentive : IDonationIncentive {
  public string Name { get; set; } = "Unnamed";

  [JsonProperty("goal")]
  public decimal Goal { get; set; }

  [JsonProperty("amount")]
  public decimal Amount { get; set; }

  [JsonProperty("percentageCompleted")]
  public decimal PercentageCompleted {
    get {
      if (Goal <= 0) {
        return 0;
      }

      var percentage = Amount / Goal * 100;
      return Math.Round(percentage, 2);
    }
  }

  public object Clone() {
    return new DonationIncentive {
      Goal = Goal, Amount = Amount
    };
  }
}