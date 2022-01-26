// ReSharper disable InconsistentNaming

namespace Core.Twitch.DTOs;

public class ChannelSubscriptionDto : ITwitchDto {
  /// The user ID for the user who subscribed to the specified channel.
  public string user_id { get; set; }

  /// The user login for the user who subscribed to the specified channel.
  public string user_login { get; set; }

  /// The user display name for the user who subscribed to the specified channel.
  public string user_name { get; set; }

  /// The requested broadcaster ID.
  public string broadcaster_user_id { get; set; }

  /// The requested broadcaster login.
  public string broadcaster_user_login { get; set; }

  /// The requested broadcaster display name.
  public string broadcaster_user_name { get; set; }

  /// The tier of the subscription. Valid values are 1000, 2000, and 3000.
  public string tier { get; set; }

  /// Whether the subscription is a gift.
  public bool is_gift { get; set; }
}