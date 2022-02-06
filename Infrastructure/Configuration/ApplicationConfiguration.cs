using System.Reflection;
using Core.Configuration;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Configuration;

public class ApplicationConfiguration : IApplicationConfiguration {
  public ApplicationConfiguration(IConfiguration config) {
    var twitchConfig = config.GetSection("Twitch");
    var streamlabsConfig = config.GetSection("Streamlabs");

    Twitch = new TwitchConfiguration(twitchConfig);
    Streamlabs = new StreamlabsConfiguration(streamlabsConfig);

    ValidateConfigurationSections();
  }

  [ValidateConfigurationSection]
  public ITwitchConfiguration Twitch { get; }

  [ValidateConfigurationSection]
  public IStreamlabsConfiguration Streamlabs { get; }

  public void Deconstruct(out ITwitchConfiguration twitch, out IStreamlabsConfiguration streamlabs) {
    twitch = Twitch;
    streamlabs = Streamlabs;
  }

  private void ValidateConfigurationSections() {
    GetType()
      .GetProperties()
      .Where(info => info.GetCustomAttribute<ValidateConfigurationSection>() is not null)
      .Select(info => info.GetValue(this)!)
      .ToList()
      .ForEach(ValidateConfiguration);
  }

  private void ValidateConfiguration(object section) {
    var type = section.GetType();
    section.GetType()
           .GetProperties()
           .Where(info =>
             info.GetCustomAttribute<ValidateConfigurationProperty>() is not null)
           .Where(property => {
             var value = (string?) property.GetValue(section);
             return string.IsNullOrEmpty(value);
           })
           .ToList()
           .ForEach(property => throw new MissingConfigurationException(property.Name, type.Name));
  }

  internal class TwitchConfiguration : ITwitchConfiguration {
    private readonly IConfiguration _config;

    public TwitchConfiguration(IConfiguration config) {
      _config = config;
    }

    [ValidateConfigurationProperty]
    public string AccessToken => _config["AccessToken"];

    [ValidateConfigurationProperty]
    public string ClientId => _config["ClientId"];

    [ValidateConfigurationProperty]
    public string ClientSecret => _config["ClientSecret"];
  }

  internal class StreamlabsConfiguration : IStreamlabsConfiguration {
    private readonly IConfiguration _config;

    public StreamlabsConfiguration(IConfiguration config) {
      _config = config;
    }

    [ValidateConfigurationProperty]
    public string RedirectUri => _config["RedirectUri"];

    [ValidateConfigurationProperty]
    public string ClientId => _config["ClientId"];

    [ValidateConfigurationProperty]
    public string ClientSecret => _config["ClientSecret"];

    [ValidateConfigurationProperty]
    public string WebsocketUri => _config["WebsocketUri"];

    [ValidateConfigurationProperty]
    public string BaseUri => _config["BaseUri"];
  }
}