namespace Core.Interfaces;

public interface IHttpClient {
  public Task<HttpResponseMessage> GetAsync(string url);
  public Task<HttpResponseMessage> GetAsync(Uri url);
  public Task<HttpResponseMessage> PostAsync(string url, HttpContent? content);
  public Task<HttpResponseMessage> PostAsync(Uri url, HttpContent? content);
}