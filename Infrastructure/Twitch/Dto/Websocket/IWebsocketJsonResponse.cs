// ReSharper disable InconsistentNaming

namespace Infrastructure.Twitch.Dto.Websocket;

public interface IWebsocketJsonResponse {
  public string type { get; set; }
  public string data { get; set; }
}