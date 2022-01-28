using Newtonsoft.Json;

namespace Infrastructure.Twitch.Dto.Websocket;

public class ChannelSubscriptionDto : ITwitchDto {
  [JsonProperty("user_name")] public string UserName { get; set; }

  [JsonProperty("display_name")] public string DisplayName { get; set; }

  [JsonProperty("channel_name")] public string ChannelName { get; set; }

  [JsonProperty("user_id")] public double UserId { get; set; }

  [JsonProperty("channel_id")] public double ChannelId { get; set; }

  [JsonProperty("time")] public DateTimeOffset Time { get; set; }

  [JsonProperty("sub_plan")] public double SubPlan { get; set; }

  [JsonProperty("sub_plan_name")] public string SubPlanName { get; set; }

  [JsonProperty("cumulative_months")] public double CumulativeMonths { get; set; }

  [JsonProperty("streak_months")] public double StreakMonths { get; set; }

  [JsonProperty("context")] public string Context { get; set; }

  [JsonProperty("is_gift")] public bool IsGift { get; set; }

  [JsonProperty("sub_message")] public SubMessageDto SubMessage { get; set; }
}

public class SubMessageDto {
  [JsonProperty("message")] public string Message { get; set; }

  [JsonProperty("emotes")] public EmoteDto[] Emotes { get; set; }
}