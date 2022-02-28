using System;
using System.Net.Http;
using System.Web;
using Core.Configuration;
using Infrastructure.Authentication;
using Infrastructure.Streamlabs.Socket.Dto;
using Infrastructure.Tests.Mocking.Http;
using Infrastructure.Tests.Utils;

namespace Infrastructure.Tests.Streamlabs;

public static class MockHttpClientExtension {
  public static MockHttpMessageHandler
    MakeStreamlabsClient(this MockHttpMessageHandler client, StreamlabsConfiguration configuration) {
    var authTokenUrl = $"{configuration.BaseUri}/token";
    var socketTokenUrl = $"{configuration.BaseUri}/socket/token";

    client.WithPost(
      authTokenUrl,
      FileHelper.FromJsonFile<AccessTokenPair>("Streamlabs/TestData/auth-token.json"),
      (uri, content) => {
        var c = (FormUrlEncodedContent) content;

        if (!c.HasQueryParameter("code")) {
          throw new Exception("Missing code parameter");
        }
      });

    client.WithGet(socketTokenUrl,
      FileHelper.FromJsonFile<StreamlabsSocketToken>("Streamlabs/TestData/socket-token.json"),
      (uri, content) => {
        if (!uri.HasQueryParameter("access_token")) {
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

public static class UrlExtension {
  public static bool HasQueryParameter(this Uri uri, string key) {
    var queryString = uri.Query;
    var query = HttpUtility.ParseQueryString(queryString);

    return query.Get(key) is not null;
  }
}