using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using Xunit.Sdk;

namespace Infrastructure.Tests.Utils;

public class JsonFileAttribute : DataAttribute {
  private readonly string _path;
  private readonly Type _type;

  public JsonFileAttribute(string path, Type type) {
    _path = path;
    _type = type;
  }


  public override IEnumerable<object[]> GetData(MethodInfo testMethod) {
    var content = FileHelper.GetFileContent(_path);
    var obj = JsonConvert.DeserializeObject(content, _type);

    yield return new[] {obj};
  }
}