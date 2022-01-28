namespace Core.Twitch.Websocket;

public interface IRequestOrResponseType {
  public string Type { get; }
}

public partial class RequestType : IRequestOrResponseType {
  private RequestType(string type) {
    Type = type;
  }

  public static RequestType Ping => new("PING");
  public static RequestType Listen => new("LISTEN");
  public string Type { get; }
}

public partial class ResponseType : IRequestOrResponseType {
  private ResponseType(string type) {
    Type = type;
  }

  public static ResponseType Ping => new("PONG");
  public static ResponseType Listen => new("RECONNECT");

  public string Type { get; }
}

// Keep IDE from refactoring partial
public partial class RequestType {
}

public partial class ResponseType {
}