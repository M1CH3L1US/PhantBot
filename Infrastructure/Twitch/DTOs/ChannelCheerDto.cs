// ReSharper disable InconsistentNaming

namespace Infrastructure.Twitch.DTOs;

public class ChannelCheerDto : ITwitchDto {
  /// The number of bits cheered
  public int bits { get; set; }

  /// The requested broadcaster ID.
  public string broadcaster_user_id { get; set; }

  /// The requested broadcaster login.
  public string broadcaster_user_login { get; set; }

  /// The requested broadcaster display name.
  public string broadcaster_user_name { get; set; }

  /// Whether the user cheered anonymously or not.
  public bool is_anonymous { get; set; }

  /// The message sent with the cheer.
  public string message { get; set; }

  /// The user ID for the user who cheered on the specified channel. This is null if is_anonymous is true.
  public string? user_id { get; set; }

  /// The user login for the user who cheered on the specified channel. This is null if is_anonymous is true.
  public string? user_login { get; set; }

  /// The user display name for the user who cheered on the specified channel. This is null if is_anonymous is true.
  public string? user_name { get; set; }
}