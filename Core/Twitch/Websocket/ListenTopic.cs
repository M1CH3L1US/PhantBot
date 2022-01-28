namespace Core.Twitch.Websocket;

public partial class ListenTopic {
  internal ListenTopic(string topic) {
    Topic = topic;
  }

  public string Topic { get; }

  public static ListenTopic BitsEvent(string channelId) {
    return new ListenTopic($"channel-bits-events-v2.{channelId}");
  }

  public static ListenTopic BitsBadgeUnlock(string channelId) {
    return new ListenTopic($"channel-bits-badge-unlocks.{channelId}");
  }

  public static ListenTopic ChannelPoints(string channelId) {
    return new ListenTopic($"channel-points-channel-v1.{channelId}");
  }

  public static ListenTopic ChannelSubscription(string channelId) {
    return new ListenTopic($"channel-subscribe-events-v1.{channelId}");
  }

  public static ListenTopic ChatModeratorAction(string userId, string channelId) {
    return new ListenTopic($"chat_moderator_actions.{userId}.{channelId}");
  }

  public static ListenTopic AutoModQueue(string moderatorId, string channelId) {
    return new ListenTopic($"automod-queue.{moderatorId}.{channelId}");
  }

  public static ListenTopic UserModerationNotification(string userId, string channelId) {
    return new ListenTopic($"user-moderation-notifications.{userId}.{channelId}");
  }

  public static ListenTopic Whisper(string userId) {
    return new ListenTopic($"whispers.{userId}");
  }
}

// Keep IDE from refactoring
public partial class ListenTopic {
}