using Core.Twitch.DTOs;
using Core.Utils;
using Mapster;

namespace Core.Twitch.MapExtensions;

public static class TwitchEventMapExtension {
  public static T ToEvent<T>(this ITwitchDto dto) where T : class {
    var nameMatchingStrategy = NameMatchingStrategy.ConvertSourceMemberName(ConvertPropertyName);
    var adapterConfig = TypeAdapterConfig<ITwitchDto, T>.NewConfig()
                                                        .NameMatchingStrategy(nameMatchingStrategy)
                                                        .Config;
    return dto.Adapt<T>(adapterConfig);
  }

  private static string ConvertPropertyName(string str) {
    // I know this is a little silly but I hate `UserName`
    var normalizedString = str.Replace("user_name", "username");
    return FormattingHelper.ToPascalCase(normalizedString);
  }
}