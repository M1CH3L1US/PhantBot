using Core.Enums;
using Core.Interfaces;

namespace Core.Streamlabs.Events;

public interface ITwitchSubscription : IDonation {
  public string Name { get; protected set; }
  public int Months { get; protected set; }
  public string Message { get; protected set; }
  public string SubscriptionPlanName { get; protected set; }
  public ChannelSubscriptionType SubscriptionType { get; protected set; }
  public ChannelSubscriptionTier SubscriptionTier { get; protected set; }
}