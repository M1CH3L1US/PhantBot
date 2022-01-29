namespace Core.Configuration;

public interface IApplicationConfiguration {
  public ITwitchConfiguration Twitch { get; }
  public IStreamlabsConfiguration Streamlabs { get; }
}

public interface ITwitchConfiguration {
  public string AccessToken { get; }
  public string ClientId { get; }
  public string ClientSecret { get; }
}

public interface IStreamlabsConfiguration {
  public string RedirectUri { get; }
  public string ClientId { get; }
  public string ClientSecret { get; }
}

[AttributeUsage(AttributeTargets.Property)]
public class ValidateConfigurationSection : Attribute {
}

[AttributeUsage(AttributeTargets.Property)]
public class ValidateConfigurationProperty : Attribute {
}

public class MissingConfigurationException : Exception {
  public MissingConfigurationException(string name, string section) : base(
    $"Missing configuration {name} in {section} configuration section") {
  }
}