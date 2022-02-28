namespace Core.Authentication;

public interface IAccessTokenStore {
  public bool HasToken(string category);
  public string? GetToken(string category);
  public void SetToken(string category, string code);
  public void RemoveToken(string category);
}