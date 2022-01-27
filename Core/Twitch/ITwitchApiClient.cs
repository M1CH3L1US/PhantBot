namespace Core.Twitch;

public interface ITwitchApiClient {
  public Task<string> GetStreamsAsync(string channel);
}