using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Converters;

namespace Core.Enums;

[JsonConverter(typeof(StringEnumConverter))]
public enum ChannelSubscriptionType {
  [EnumMember(Value = "sub")]
  Default,

  [EnumMember(Value = "resub")]
  ReSubscription,

  [EnumMember(Value = "subgift")]
  Gifted,

  [EnumMember(Value = "anonsubgift")]
  AnonymousGifted,

  [EnumMember(Value = "resubgift")]
  ReSubscriptionGifted,

  [EnumMember(Value = "anonresubgift")]
  AnonymousReSubscriptionGifted
}