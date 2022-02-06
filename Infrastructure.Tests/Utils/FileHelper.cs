using System.IO;
using Newtonsoft.Json;

namespace Infrastructure.Tests.Utils;

public class FileHelper {
  public static string GetFileContent(string path) {
    var filePath = GetFilePath(path);
    return File.ReadAllText(filePath);
  }

  public static T FromJsonFile<T>(string path) {
    var filePath = GetFilePath(path);
    var fileContent = GetFileContent(filePath);

    return JsonConvert.DeserializeObject<T>(fileContent)!;
  }

  public static string GetFilePath(string path) {
    var filePath =
      Path.IsPathRooted(path) ? path : GetRelativePath(path);

    if (!File.Exists(filePath)) {
      throw new FileNotFoundException($"File {filePath} not found");
    }

    return filePath;
  }


  private static string GetRelativePath(string path) {
    var currentDirectory = Directory.GetCurrentDirectory();
    var relativeFromCurrent = Path.GetRelativePath(currentDirectory, path);

    return Path.Combine(currentDirectory, relativeFromCurrent);
  }
}