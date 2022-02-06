using System;
using System.Collections.Generic;
using Core.Authentication;

namespace Infrastructure.Tests.Authentication;

public class MockAuthenticationCodeStore : IAuthenticationCodeStore {
  public IDictionary<string, string> Codes = new Dictionary<string, string>();

  public bool HasAuthenticationCode(string category) {
    return Codes.ContainsKey(category);
  }

  public string? GetAuthenticationCode(string category) {
    return Codes[category];
  }

  public void SetAuthenticationCode(string category, string code) {
    Codes[category] = code;
  }

  public void RemoveAuthenticationCode(string category) {
    Codes.Remove(category);
  }

  public MockAuthenticationCodeStore Add(string category) {
    SetAuthenticationCode(category, Guid.NewGuid().ToString());
    return this;
  }

  public static MockAuthenticationCodeStore Create(string category) {
    var store = new MockAuthenticationCodeStore();
    store.SetAuthenticationCode(category, Guid.NewGuid().ToString());
    return store;
  }
}