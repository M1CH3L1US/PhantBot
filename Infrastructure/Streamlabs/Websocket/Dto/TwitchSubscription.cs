using Core.Enums;
using Core.Streamlabs.Events;
using Infrastructure.Shared.Typing;
using Newtonsoft.Json;

namespace Infrastructure.Streamlabs.Websocket.Dto;

public class TwitchSubscription : ITwitchSubscription, IEventDto {
  public string EventName { get; } = "subscription";

  [JsonProperty("name")]
  string ITwitchSubscription.Name { get; set; }

  [JsonProperty("months")]
  int ITwitchSubscription.Months { get; set; }

  [JsonProperty("message")]
  string ITwitchSubscription.Message { get; set; }

  [JsonProperty("sub_plan_name")]
  string ITwitchSubscription.SubscriptionPlanName { get; set; }

  [JsonProperty("sub_type")]
  ChannelSubscriptionType ITwitchSubscription.SubscriptionType { get; set; }

  [JsonProperty("sub_plan")]
  ChannelSubscriptionTier ITwitchSubscription.SubscriptionTier { get; set; }
}