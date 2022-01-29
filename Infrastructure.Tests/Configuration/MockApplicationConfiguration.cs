using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace Infrastructure.Test.Configuration;

public class MockApplicationConfiguration : IConfigurationSection {
  public Dictionary<string, string> Values = new();

  public MockApplicationConfiguration(string? sectionName = null) {
    if (sectionName is not null) {
      Key = sectionName;
      return;
    }

    MakeAndAddSection("Twitch", section => {
      section.Values.Add("AccessToken", GetGuid());
      section.Values.Add("ClientId", GetGuid());
      section.Values.Add("ClientSecret", GetGuid());
    });

    MakeAndAddSection("Streamlabs", section => {
      section.Values.Add("RedirectUri", GetGuid());
      section.Values.Add("ClientId", GetGuid());
      section.Values.Add("ClientSecret", GetGuid());
    });
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

  public void MakeAndAddSection(string key, Action<MockApplicationConfiguration> callback) {
    MockApplicationConfiguration section = new(key);
    Sections[key] = section;
    callback(section);
  }

  public static MockApplicationConfiguration Create() {
    return new MockApplicationConfiguration();
  }

  private string GetGuid() {
    return Guid.NewGuid().ToString();
  }
}