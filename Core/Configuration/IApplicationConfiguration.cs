namespace Core.Configuration;

public interface IApplicationConfiguration {
  public string AccessToken { get; }
  public string ClientId { get; }
  public string ClientSecret { get; }
}

[AttributeUsage(AttributeTargets.Property)]
public class ValidateConfigurationProperty : Attribute {
}

public class MissingConfigurationException : Exception {
  public MissingConfigurationException(string name) : base(
    $"Missing configuration {name} in Twitch configuration section") {
  }
}