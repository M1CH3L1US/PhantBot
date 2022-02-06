using Core.Authentication;
using Newtonsoft.Json;

namespace Infrastructure.Authentication;

public class AccessTokenPair : IAccessTokenPair {
  [JsonProperty("access_token")]
  public string AccessToken { get; }

  [JsonProperty("refresh_token")]
  public string RefreshToken { get; }

  [JsonProperty("token_type")]
  public string TokenType { get; }

  [JsonProperty("expires_in")]
  public int ExpiresIn { get; }
}