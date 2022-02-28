using Core.Authentication;

namespace Infrastructure.Authentication.Stores;

public class EnvironmentAccessTokenStore : IAccessTokenStore {
  private const string EnvironmentKeyPrefix = "__PHANT__BOT__AUTH__TOKEN";

  // Environment variables are loaded once, when the process is started.
  // To get access to the token after it has been set in the environment,
  // we need to store in in memory as well to make it available before the app
  // is restarted.
  internal readonly Dictionary<string, string> ProcessEnvironmentStore = new();

  public bool HasToken(string category) {
    var code = GetToken(category);
    return !string.IsNullOrEmpty(code);
  }

  public string? GetToken(string category) {
    var key = GetEnvironmentKey(category);
    return Environment.GetEnvironmentVariable(key) ?? ProcessEnvironmentStore.GetValueOrDefault(key);
  }

  public void SetToken(string category, string code) {
    var key = GetEnvironmentKey(category);
    SetEnvironmentVariable(key, code);
  }

  public void RemoveToken(string category) {
    var key = GetEnvironmentKey(category);
    SetEnvironmentVariable(key, null);
  }

  private void SetEnvironmentVariable(string key, string? value) {
    Environment.SetEnvironmentVariable(key, value, EnvironmentVariableTarget.User);
    ProcessEnvironmentStore[key] = value;
  }

  private string GetEnvironmentKey(string category) {
    var categoryKey = category.ToUpper();
    return $"{EnvironmentKeyPrefix}__{categoryKey}__";
  }
}