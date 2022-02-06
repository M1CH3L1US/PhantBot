namespace Core.Authentication;

public interface IAuthClient {
  /// <summary>
  ///   The server application will call this method
  ///   when the user authorizes the streamlabs
  ///   Application. This will likely only happen once,
  ///   when the app is started for the first time.
  /// </summary>
  public Task NotifyAuthorizationCodeObtained();

  public Task ObtainAccessTokenPair();
  public Task<string> GetAccessToken();
  public Task RefreshAccessToken();
}