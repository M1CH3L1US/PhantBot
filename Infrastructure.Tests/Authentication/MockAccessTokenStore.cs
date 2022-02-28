using System;
using System.Collections.Generic;
using Core.Authentication;

namespace Infrastructure.Tests.Authentication;

public class MockAccessTokenStore : IAccessTokenStore {
  public IDictionary<string, string> Codes = new Dictionary<string, string>();

  public bool HasToken(string category) {
    return Codes.ContainsKey(category);
  }

  public string? GetToken(string category) {
    return Codes[category];
  }

  public void SetToken(string category, string code) {
    Codes[category] = code;
  }

  public void RemoveToken(string category) {
    Codes.Remove(category);
  }

  public MockAccessTokenStore Add(string category) {
    SetToken(category, Guid.NewGuid().ToString());
    return this;
  }

  public static MockAccessTokenStore Create(string category) {
    var store = new MockAccessTokenStore();
    store.SetToken(category, Guid.NewGuid().ToString());
    return store;
  }
}