namespace Core.Twitch.Events;

public interface IChannelCheerEvent : ITwitchEvent {
  /// The number of bits cheered
  public int Bits { get; set; }

  /// The requested broadcaster ID.
  public string BroadcasterUserId { get; set; }

  /// The requested broadcaster login.
  public string BroadcasterUserLogin { get; set; }

  /// The requested broadcaster display name.
  public string BroadcasterUsername { get; set; }

  /// Whether the user cheered anonymously or not.
  public bool IsAnonymous { get; set; }

  /// The message sent with the cheer.
  public string Message { get; set; }

  /// The user ID for the user who cheered on the specified channel. This is null if IsAnonymous is true.
  public string? UserId { get; set; }

  /// The user login for the user who cheered on the specified channel. This is null if IsAnonymous is true.
  public string? UserLogin { get; set; }

  /// The user display name for the user who cheered on the specified channel. This is null if IsAnonymous is true.
  public string? Username { get; set; }
}