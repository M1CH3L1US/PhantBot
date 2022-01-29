using System.Reflection;
using Core.Configuration;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Configuration;

public class ApplicationConfiguration : IApplicationConfiguration {
  private readonly IConfigurationSection _config;

  public ApplicationConfiguration(IConfiguration config) {
    _config = config.GetSection("Twitch");
    ValidateConfiguration();
  }

  [ValidateConfigurationProperty]
  public string AccessToken => _config["AccessToken"];

  [ValidateConfigurationProperty]
  public string ClientId => _config["ClientId"];

  [ValidateConfigurationProperty]
  public string ClientSecret => _config["ClientSecret"];

  private void ValidateConfiguration() {
    var propertiesToValidate = GetType()
                               .GetProperties()
                               .Where(info => info.GetCustomAttribute<ValidateConfigurationProperty>() is not null)
                               .ToList();

    foreach (var property in propertiesToValidate) {
      var value = (string?) property.GetValue(this);

      if (string.IsNullOrEmpty(value)) {
        throw new MissingConfigurationException(property.Name);
      }
    }
  }
}