namespace Core.Streamlabs;

public class StreamlabsTokenNameRegistry {
  private const string _prefix = "streamlabs";
  public static string AccessToken = $"{_prefix}_access_token";
  public static string RefreshToken = $"{_prefix}_refresh_token";
  public static string AutorizationCode = $"{_prefix}_auth_code";
}