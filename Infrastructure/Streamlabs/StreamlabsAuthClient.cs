using Core.Authentication;
using Core.Configuration;
using Core.Streamlabs;
using Infrastructure.Authentication;
using Infrastructure.Streamlabs.Websocket.Dto;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Infrastructure.Streamlabs;

public class StreamlabsAuthClient : IStreamlabsAuthClient {
  private const string Category = "Streamlabs";
  internal readonly IAuthenticationCodeStore AuthenticationCodeStore;
  internal readonly StreamlabsConfiguration Configuration;
  internal readonly HttpClient HttpClient;

  internal string? SocketAccessToken;
  internal IAccessTokenPair? TokenPair;

  public StreamlabsAuthClient(
    HttpClient httpClient,
    IOptions<StreamlabsConfiguration> configuration,
    IAuthenticationCodeStore authenticationCodeStore
  ) {
    Configuration = configuration.Value;
    HttpClient = httpClient;
    AuthenticationCodeStore = authenticationCodeStore;
  }

  public Task NotifyAuthorizationCodeObtained() {
    return Task.CompletedTask;
  }

  public Task ObtainAccessTokenPair() {
    return FetchAccessTokenPair();
  }

  public async Task<string> GetSocketAuthToken() {
    if (!string.IsNullOrEmpty(SocketAccessToken)) {
      return SocketAccessToken;
    }

    if (!HasTokenPair()) {
      await FetchAccessTokenPair();
    }

    SocketAccessToken = await FetchSocketToken();
    return SocketAccessToken;
  }

  public async Task<string> GetAccessToken() {
    if (HasTokenPair()) {
      return TokenPair!.AccessToken;
    }

    await FetchAccessTokenPair();
    return TokenPair!.AccessToken;
  }

  public Task RefreshAccessToken() {
    if (!HasTokenPair()) {
      throw new Exception("Can't refresh access token before a token is initially obtained");
    }

    var refreshToken = TokenPair!.RefreshToken;
    return FetchAccessTokenPair(refreshToken);
  }

  private async Task<string> FetchSocketToken() {
    var url = $"{Configuration.BaseUri}/socket/token";
    var accessToken = TokenPair!.AccessToken;

    var body = new HttpRequestBody();
    body.Add("access_token", accessToken);
    var content = body.ToFormUrlEncodedContent();

    var response = await HttpClient.PostAsync(url, content);
    var responseBody = await response.Content.ReadAsStringAsync();
    var tokenResponse = JsonConvert.DeserializeObject<StreamlabsSocketToken>(responseBody)!;

    return tokenResponse.Token;
  }

  private async Task FetchAccessTokenPair(string? refreshToken = null) {
    var body = GetAuthenticationBody(refreshToken);
    var content = body.ToFormUrlEncodedContent();
    var responseMessage = await HttpClient.PostAsync($"{Configuration.BaseUri}/token", content);

    var responseBody = await responseMessage.Content.ReadAsStringAsync();
    var tokenPair = JsonConvert.DeserializeObject<AccessTokenPair>(responseBody);

    TokenPair = tokenPair;
  }

  private HttpRequestBody GetAuthenticationBody(string? refreshToken = null) {
    var body = new HttpRequestBody();
    var grantType = GetGrantType(refreshToken);
    var authenticationCode = AuthenticationCodeStore.GetAuthenticationCode(Category);

    if (authenticationCode is null) {
      throw new Exception("Cannot obtain auth token before obtaining authentication code");
    }

    if (!string.IsNullOrEmpty(refreshToken)) {
      body.Add("refresh_token", refreshToken);
    }

    body.Add("grant_type", grantType);
    body.Add("client_id", Configuration.ClientId);
    body.Add("client_secret", Configuration.ClientSecret);
    body.Add("redirect_uri", Configuration.RedirectUri);
    body.Add("code", authenticationCode);

    return body;
  }

  private bool HasTokenPair() {
    return TokenPair is not null;
  }

  private string GetGrantType(string? refreshToken) {
    return refreshToken is null ? "authorization_code" : "refresh_token";
  }

  internal class HttpRequestBody : List<KeyValuePair<string, string>> {
    public void Add(string key, string value) {
      base.Add(new KeyValuePair<string, string>(key, value));
    }

    public FormUrlEncodedContent ToFormUrlEncodedContent() {
      return new FormUrlEncodedContent(this);
    }
  }
}