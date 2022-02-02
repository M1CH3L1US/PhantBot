using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit.Sdk;

namespace Infrastructure.Test.Utils;

[AttributeUsage(AttributeTargets.Method)]
public class FileContentAttribute : DataAttribute {
  private readonly string _filePath;

  public FileContentAttribute(string filePath) {
    _filePath = filePath;
  }

  public override IEnumerable<object[]> GetData(MethodInfo testMethod) {
    var textContent = FileHelper.GetFileContent(_filePath);
    yield return new object[] {textContent};
  }
}