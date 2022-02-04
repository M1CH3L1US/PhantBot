namespace Core.Interfaces;

public interface IHttpClient {
  public Task<HttpResponseMessage> GetAsync(string url);
  public Task<HttpResponseMessage> GetAsync(Uri url);
}