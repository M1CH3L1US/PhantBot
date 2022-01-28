namespace Core.Twitch.Websocket;

public static class WebsocketRequestBuilder {
  public static object BuildRequest(IRequestOrResponseType requestType, object? requestData = null) {
    if (requestData is null)
      return new {
        type = requestType.Type
      };

    return new {
      type = requestType.Type,
      data = requestData
    };
  }
}