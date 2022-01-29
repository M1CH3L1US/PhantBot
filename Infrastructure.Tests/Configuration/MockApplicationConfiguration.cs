using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace Infrastructure.Test.Configuration;

public class MockApplicationConfiguration : IConfigurationSection {
  public Dictionary<string, string> Values = new();

  public MockApplicationConfiguration(string? section = null) {
    if (section is not null) {
      Key = section;
      return;
    }

    var twtichConfigurationSection = new MockApplicationConfiguration("Twitch");
    Sections["Twitch"] = twtichConfigurationSection;

    twtichConfigurationSection.Values.Add("AccessToken", GetGuid());
    twtichConfigurationSection.Values.Add("ClientId", GetGuid());
    twtichConfigurationSection.Values.Add("ClientSecret", GetGuid());
  }

  public Dictionary<string, IConfigurationSection> Sections { get; set; } = new();

  public IConfigurationSection GetSection(string key) {
    return Sections[key];
  }

  public IEnumerable<IConfigurationSection> GetChildren() {
    return null;
  }

  public IChangeToken GetReloadToken() {
    return null;
  }

  public string this[string key] {
    get => Values[key];
    set => Values[key] = value;
  }

  public string? Key { get; }
  public string? Path { get; }
  public string? Value { get; set; }

  public static MockApplicationConfiguration Create() {
    return new MockApplicationConfiguration();
  }

  private string GetGuid() {
    return Guid.NewGuid().ToString();
  }
}