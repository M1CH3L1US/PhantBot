using System.Text.RegularExpressions;

namespace Core.Utils;

public static class FormattingHelper {
  public static string ToPascalCase(string str) {
    return Regex.Replace(str, "(^|_)[a-z]", m => m.ToString().TrimStart('_').ToUpper());
  }
}