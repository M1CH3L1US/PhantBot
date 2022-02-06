using System.Collections.Generic;
using Core.Authentication;
using Core.Configuration;
using Core.Interfaces;
using Core.Streamlabs;
using Infrastructure.Shared.Typing;
using Infrastructure.Streamlabs;
using Infrastructure.Streamlabs.Websocket;
using Infrastructure.Streamlabs.Websocket.Dto;
using Infrastructure.Tests.Authentication;
using Infrastructure.Tests.Configuration;
using Infrastructure.Tests.Mocking.Http;
using Infrastructure.Tests.Streamlabs;
using Infrastructure.Tests.Utils;
using Microsoft.Extensions.DependencyInjection;
using Websocket.Client;

namespace Infrastructure.Tests;

public class Startup {
  public void ConfigureServices(IServiceCollection services) {
    services.AddSingleton(MockApplicationConfiguration.Create());
    services.AddTransient<IStreamlabsWebsocketClient, StreamlabsWebsocketClient>();
    services.AddTransient<IWebsocketClient, MockWebSocketClient>();
    services.AddTransient<MockWebSocketClient>();
    services.AddTransient<IStreamlabsAuthClient, StreamlabsAuthClient>();
    services.AddTransient<IHttpClient>(serviceProvider => {
      var configuration = serviceProvider.GetRequiredService<IApplicationConfiguration>();
      return MockHttpClient.Configure()
                           .MakeStreamlabsClient(configuration);
    });
    services.AddSingleton<IAuthenticationCodeStore>(_ => MockAuthenticationCodeStore
                                                         .Create("Streamlabs")
                                                         .Add("Twitch")
    );
    services.AddSingleton<IEventDtoContainer>(_ => {
      var events = new List<IEventDto> {
        new StreamlabsDonation(),
        new TwitchSubscription(),
        new TwitchBitsCheer()
      };

      return new EventDtoContainer(events);
    });
  }
}