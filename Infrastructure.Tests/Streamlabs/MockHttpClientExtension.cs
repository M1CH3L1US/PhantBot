using System;
using System.Net.Http;
using System.Web;
using Core.Configuration;
using Infrastructure.Authentication;
using Infrastructure.Streamlabs.Websocket.Dto;
using Infrastructure.Tests.Mocking.Http;
using Infrastructure.Tests.Utils;

namespace Infrastructure.Tests.Streamlabs;

public static class MockHttpClientExtension {
  public static MockHttpClient
    MakeStreamlabsClient(this MockHttpClient client, IApplicationConfiguration configuration) {
    var (_, streamlabs) = configuration;
    var authTokenUrl = $"{streamlabs.BaseUri}/token";
    var socketTokenUrl = $"{streamlabs.BaseUri}/socket/token";

    client.WithPost(
      authTokenUrl,
      FileHelper.FromJsonFile<AccessTokenPair>("Streamlabs/TestData/auth-token.json"),
      (uri, content) => {
        var c = (FormUrlEncodedContent) content;

        if (!c.HasQueryParameter("code")) {
          throw new Exception("Missing code parameter");
        }
      });

    client.WithPost(socketTokenUrl,
      FileHelper.FromJsonFile<StreamlabsSocketToken>("Streamlabs/TestData/socket-token.json"),
      (uri, content) => {
        var c = (FormUrlEncodedContent) content;

        if (!c.HasQueryParameter("access_token")) {
          throw new Exception("Missing Auth token in query string");
        }
      });

    return client;
  }
}

public static class FormUrlEncodedContentExtension {
  public static bool HasQueryParameter(this FormUrlEncodedContent uri, string key) {
    var queryString = uri.ReadAsStringAsync().ToSynchronous();
    var query = HttpUtility.ParseQueryString(queryString);

    return query.Get(key) is not null;
  }
}