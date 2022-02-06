namespace Core.Authentication;

public interface IAccessTokenPair {
  string AccessToken { get; }
  string RefreshToken { get; }
  string TokenType { get; }

  int ExpiresIn { get; }
}