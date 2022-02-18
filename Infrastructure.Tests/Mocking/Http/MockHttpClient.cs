using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Infrastructure.Tests.Mocking.Http;

internal class MockHttpResponseHandler {
  public Uri URI { get; set; }
  public HttpResponseMessage Response { get; set; }
  public HttpMethod Method { get; set; }
  public Action<Uri, HttpContent>? ValidateRequest { get; set; }
}

public class MockHttpMessageHandler : HttpMessageHandler {
  private readonly List<MockHttpResponseHandler> _responseMessages = new();

  private MockHttpMessageHandler() {
  }

  public static MockHttpMessageHandler Configure() {
    return new MockHttpMessageHandler();
  }

  public MockHttpMessageHandler WithGet<T>(string url, T response) {
    var res = MakeResponse(response);
    _responseMessages.Add(new MockHttpResponseHandler {
      URI = new Uri(url),
      Response = res,
      Method = HttpMethod.Get
    });

    return this;
  }

  public MockHttpMessageHandler WithPost<T>(string url, T response, Action<Uri, HttpContent> validator) {
    var res = MakeResponse(response);
    _responseMessages.Add(new MockHttpResponseHandler {
      URI = new Uri(url),
      Response = res,
      Method = HttpMethod.Post,
      ValidateRequest = validator
    });

    return this;
  }


  public MockHttpMessageHandler WithPost<T>(string url, T response) {
    var res = MakeResponse(response);

    _responseMessages.Add(new MockHttpResponseHandler {
      URI = new Uri(url),
      Response = res,
      Method = HttpMethod.Post
    });

    return this;
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

  protected override async Task<HttpResponseMessage> SendAsync(
    HttpRequestMessage request,
    CancellationToken cancellationToken
  ) {
    var handler = _responseMessages
      .FirstOrDefault(
        handler => handler.Method == request.Method &&
                   handler.URI == request.RequestUri
      );

    if (handler is null) {
      throw new HttpRequestException($"No handler found for request to url {request.RequestUri}");
    }

    handler.ValidateRequest?.Invoke(request.RequestUri, request.Content);
    return handler.Response;
  }
}