using Core.Authentication;

namespace Infrastructure.Authentication.Stores;

public class EnvironmentTokenStore : IAuthenticationCodeStore {
  private const string EnvironmentKeyPrefix = "__PHANT__BOT__AUTH__TOKEN__";

  public bool HasAuthenticationCode(string category) {
    var code = GetAuthenticationCode(category);
    return !string.IsNullOrEmpty(code);
  }

  public string? GetAuthenticationCode(string category) {
    var key = GetEnvironmentKey(category);
    return Environment.GetEnvironmentVariable(key);
  }

  public void SetAuthenticationCode(string category, string code) {
    var key = GetEnvironmentKey(category);
    SetEnvironmentVariable(key, code);
  }

  public void RemoveAuthenticationCode(string category) {
    var key = GetEnvironmentKey(category);
    SetEnvironmentVariable(key, null);
  }

  private void SetEnvironmentVariable(string key, string? value) {
    Environment.SetEnvironmentVariable(key, value, EnvironmentVariableTarget.User);
  }

  private string GetEnvironmentKey(string category) {
    return $"{EnvironmentKeyPrefix}{category}__";
  }
}