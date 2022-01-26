namespace Core.Twitch.Events;

public interface IChannelSubscriptionEvent : ITwitchEvent {
  /// The user ID for the user who subscribed to the specified channel.
  public string UserId { get; set; }

  /// The user login for the user who subscribed to the specified channel.
  public string UserLogin { get; set; }

  /// The user display name for the user who subscribed to the specified channel.
  public string Username { get; set; }

  /// The requested broadcaster ID.
  public string BroadcasterUserId { get; set; }

  /// The requested broadcaster login.
  public string BroadcasterUserLogin { get; set; }

  /// The requested broadcaster display name.
  public string BroadcasterUsername { get; set; }

  /// The tier of the subscription. Valid values are 1000, 2000, and 3000.
  public ChannelSubscriptionTier Tier { get; set; }

  /// Whether the subscription is a gift.
  public bool IsGift { get; set; }
}