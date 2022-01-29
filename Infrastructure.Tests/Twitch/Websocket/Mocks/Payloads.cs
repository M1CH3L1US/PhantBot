using System.Collections.Generic;
using System.Linq;
using Core.Configuration;
using Core.Twitch.Websocket;

namespace Infrastructure.Test.Twitch.Websocket.Mocks;

public class Payloads {
  private readonly IApplicationConfiguration _configuration;

  public readonly object BitsMessage = new {
    type = "MESSAGE",
    data = new {
      topic = "channel-bits-events-v2.46024993",
      message = new {
        data = new {
          user_name = "jwp",
          channel_name = "bontakun",
          user_id = "95546976",
          channel_id = "46024993",
          time = "2017-02-09T13:23:58.168Z",
          chat_message = "cheer10000 New badge hype!",
          bits_used = 10000,
          total_bits_used = 25000,
          context = "cheer",
          badge_entitlement = new {
            new_version = 25000, previous_version = 10000
          }
        },
        version =
          "1.0",
        message_type =
          "bits_event",
        message_id =
          "8145728a4-35f0-4cf7-9dc0-f2ef24de1eb6",
        is_anonymous =
          true
      }
    }
  };

  public readonly object PingRequest = WebsocketRequestBuilder.BuildRequest(RequestType.Ping);
  public readonly object PingResponse = WebsocketRequestBuilder.BuildRequest(ResponseType.Ping);

  public Payloads(IApplicationConfiguration configuration) {
    _configuration = configuration;
  }

  public object ListenRequest(IEnumerable<ListenTopic> topics) {
    return new {
      type = "LISTEN",
      data = new {
        topics = topics.Select(t => t.Topic),
        auth_token = _configuration.Twitch.AccessToken
      }
    };
  }
}