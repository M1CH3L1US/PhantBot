namespace Core.Twitch.Websocket;

public static class ListenTopicStringBuilder {
  public static IEnumerable<string> GetTopicStrings(IEnumerable<ListenTopic> listenTopics) {
    return listenTopics.Select(topic => topic.GetString());
  }
}