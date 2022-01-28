namespace Core.Twitch.Websocket;

public class WebsocketRequestBuilder {
  public static object BuildRequest(IRequestOrResponseType requestType, object? requestData) {
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