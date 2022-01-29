using Core.Configuration;
using Infrastructure.Configuration;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Infrastructure.Test.Configuration;

public class ApplicationConfigurationTest {
  private readonly IConfiguration _configuration;
  private readonly IApplicationConfiguration _sut;

  public ApplicationConfigurationTest() {
    _configuration = MockApplicationConfiguration.Create();
    _sut = new ApplicationConfiguration(_configuration);
  }

  [Fact]
  public void Constructor_ThrowsMissingConfigurationException_WhenRequiredConfigurationIsMissing() {
    IConfiguration emptyConfiguration = new ConfigurationBuilder().Build();

    Assert.Throws<MissingConfigurationException>(
      () => new ApplicationConfiguration(emptyConfiguration)
    );
  }

  [Fact]
  public void Getter_ReturnsAppropriateConfigurationFromTwitchConfigSection() {
    var twitchSection = _configuration.GetSection("Twitch");
    var expectedToken = twitchSection["AccessToken"];
    var actualToken = _sut.AccessToken;

    Assert.Equal(expectedToken, actualToken);
  }
}