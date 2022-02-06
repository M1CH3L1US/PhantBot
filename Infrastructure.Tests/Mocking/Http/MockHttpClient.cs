using System;
using System.Net.Http;
using System.Threading.Tasks;
using Core.Interfaces;
using Moq;
using Newtonsoft.Json;

namespace Infrastructure.Tests.Mocking.Http;

public class MockHttpClient : IHttpClient {
  public readonly Mock<IHttpClient> MockInstance;

  private MockHttpClient() {
    MockInstance = new Mock<IHttpClient>();
  }

  public IHttpClient Instance => MockInstance.Object;

  public Task<HttpResponseMessage> GetAsync(string url) {
    return Instance.GetAsync(url);
  }

  public Task<HttpResponseMessage> GetAsync(Uri url) {
    return Instance.GetAsync(url);
  }

  public Task<HttpResponseMessage> PostAsync(string url, HttpContent? content) {
    return Instance.PostAsync(url, content);
  }

  public Task<HttpResponseMessage> PostAsync(Uri url, HttpContent? content) {
    return Instance.PostAsync(url, content);
  }

  public static MockHttpClient Configure() {
    return new MockHttpClient();
  }

  public MockHttpClient WithGet<T>(string url, T response) {
    var res = MakeResponse(response);

    MockInstance.Setup(x => x.GetAsync(url)).ReturnsAsync(res);

    return this;
  }

  public MockHttpClient WithPost<T>(string url, T response, Action<Uri, HttpContent> validator) {
    var res = MakeResponse(response);

    MockInstance.Setup(x => x.PostAsync(url, It.IsAny<HttpContent>()))
                .Callback<string, HttpContent>((url, content) => validator(new Uri(url), content))
                .ReturnsAsync(res);

    return this;
  }


  public MockHttpClient WithPost<T>(string url, T response) {
    var res = MakeResponse(response);

    MockInstance.Setup(x => x.PostAsync(url, It.IsAny<HttpContent>()))
                .ReturnsAsync(res);
    return this;
  }

  private Uri MakeUri(string uri) {
    return new Uri(uri);
  }

  private Uri MakeUri(Uri uri) {
    return uri;
  }

  private HttpResponseMessage MakeResponse<T>(T value) {
    string messageContent;

    if (value is string s) {
      messageContent = s;
    }
    else {
      messageContent = JsonConvert.SerializeObject(value);
    }

    return new HttpResponseMessage {
      Content = new StringContent(messageContent)
    };
  }
}