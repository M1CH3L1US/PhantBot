namespace Core.Twitch.Websocket;

public abstract class ListenTopic {
  protected string? ChannelId;
  protected string? UserId;
  internal abstract string GetString();
}

public class BitsEventTopic : ListenTopic {
  public BitsEventTopic(string channelId) {
    ChannelId = channelId;
  }

  internal override string GetString() {
    return $"channel-bits-events-v2.{ChannelId}";
  }
}

public class BitsBadgeUnlockTopic : ListenTopic {
  public BitsBadgeUnlockTopic(string channelId) {
    ChannelId = channelId;
  }

  internal override string GetString() {
    return $"channel-bits-badge-unlocks.{ChannelId}";
  }
}

public class ChannelPointsTopic : ListenTopic {
  public ChannelPointsTopic(string channelId) {
    ChannelId = channelId;
  }

  internal override string GetString() {
    return $"channel-points-channel-v1.{ChannelId}";
  }
}

public class ChannelSubscriptionTopic : ListenTopic {
  public ChannelSubscriptionTopic(string channelId) {
    ChannelId = channelId;
  }

  internal override string GetString() {
    return $"channel-subscribe-events-v1.{ChannelId}";
  }
}

public class ChatModeratorActionTopic : ListenTopic {
  public ChatModeratorActionTopic(string userId, string channelId) {
    UserId = userId;
    ChannelId = channelId;
  }

  internal override string GetString() {
    return $"chat_moderator_actions.{UserId}.{ChannelId}";
  }
}

public class AutoModQueueTopic : ListenTopic {
  public AutoModQueueTopic(string moderatorId, string channelId) {
    UserId = moderatorId;
    ChannelId = channelId;
  }

  internal override string GetString() {
    return $"automod-queue.{UserId}.{ChannelId}";
  }
}

public class UserModerationNotificationTopic : ListenTopic {
  public UserModerationNotificationTopic(string userId, string channelId) {
    UserId = userId;
    ChannelId = channelId;
  }

  internal override string GetString() {
    return $"user-moderation-notifications.{UserId}.{ChannelId}";
  }
}

public class WhisperTopic : ListenTopic {
  public WhisperTopic(string userId) {
    UserId = userId;
  }

  internal override string GetString() {
    return $"whispers.{UserId}";
  }
}