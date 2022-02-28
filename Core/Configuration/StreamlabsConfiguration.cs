namespace Core.Configuration;

public class StreamlabsConfiguration {
  public string RedirectUri { get; set; }
  public string ClientId { get; set; }
  public string ClientSecret { get; set; }
  public string WebsocketUri { get; set; }
  public string BaseUri { get; set; }
}