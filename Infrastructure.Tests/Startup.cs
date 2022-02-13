using System.Collections.Generic;
using Core.Authentication;
using Core.Configuration;
using Core.Finance;
using Core.Interfaces;
using Core.Streamlabs;
using Infrastructure.Finance;
using Infrastructure.Shared.Typing;
using Infrastructure.Streamlabs;
using Infrastructure.Streamlabs.Websocket;
using Infrastructure.Streamlabs.Websocket.Dto;
using Infrastructure.Tests.Authentication;
using Infrastructure.Tests.Configuration;
using Infrastructure.Tests.Finance;
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
    services.AddSingleton<ICurrencyConverter, EuropeanBankCurrencyConverter>();
    services.AddTransient<IHttpClient>(provider => {
      var configuration = provider.GetRequiredService<IApplicationConfiguration>();
      return MockHttpClient.Configure()
                           .MakeStreamlabsClient(configuration)
                           .MakeFinanceClient();
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
    services.AddSingleton<IDonationConverter>(provider => {
      var currencyConverter = provider.GetRequiredService<ICurrencyConverter>();
      return new DonationConverter(currencyConverter, Currency.Usd);
    });
  }
}