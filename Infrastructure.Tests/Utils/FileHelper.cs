using System.IO;

namespace Infrastructure.Test.Utils;

public class FileHelper {
  public static string GetFileContent(string path) {
    var filePath =
      Path.IsPathRooted(path) ? path : GetRelativePath(path);

    if (!File.Exists(filePath)) {
      throw new FileNotFoundException($"File {filePath} not found");
    }

    return File.ReadAllText(filePath);
  }

  private static string GetRelativePath(string path) {
    var currentDirectory = Directory.GetCurrentDirectory();
    var relativeFromCurrent = Path.GetRelativePath(currentDirectory, path);

    return Path.Combine(currentDirectory, relativeFromCurrent);
  }
}