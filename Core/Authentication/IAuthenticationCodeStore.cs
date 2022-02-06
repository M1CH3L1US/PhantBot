namespace Core.Authentication;

public interface IAuthenticationCodeStore {
  public bool HasAuthenticationCode(string category);
  public string? GetAuthenticationCode(string category);
  public void SetAuthenticationCode(string category, string code);
  public void RemoveAuthenticationCode(string category);
}