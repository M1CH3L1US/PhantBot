using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Net.Http;
using Core.Authentication;
using Core.Configuration;
using Core.Finance;
using Core.Streamlabs;
using Infrastructure.Finance;
using Infrastructure.Shared.Typing;
using Infrastructure.Streamlabs;
using Infrastructure.Streamlabs.Socket;
using Infrastructure.Streamlabs.Socket.Dto;
using Infrastructure.Tests.Authentication;
using Infrastructure.Tests.Finance;
using Infrastructure.Tests.Mocking;
using Infrastructure.Tests.Mocking.Http;
using Infrastructure.Tests.Streamlabs;
using Infrastructure.Tests.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Socket.Io.Client.Core;
using Websocket.Client;

namespace Infrastructure.Tests;

public class Startup {
  public void ConfigureServices(IServiceCollection services) {
    var configuration = new ConfigurationBuilder()
                        .SetBasePath(Environment.CurrentDirectory)
                        .AddJsonFile("appsettings.json", false)
                        .Build();

    services.Configure<StreamlabsConfiguration>(configuration.GetRequiredSection("Streamlabs"));
    services.Configure<TwitchConfiguration>(configuration.GetRequiredSection("Twitch"));
    services.Configure<StorageConfiguration>(configuration.GetRequiredSection("Storage"));

    services.AddTransient<IStreamlabsSocketClient, StreamlabsSocketClient>();
    services.AddTransient<IWebsocketClient, MockWebSocketClient>();
    services.AddTransient<MockWebSocketClient>();
    services.AddTransient<ISocketIoClient, MockSocketIoClient>();
    services.AddTransient<IStreamlabsAuthClient, StreamlabsAuthClient>();
    services.AddSingleton<ICurrencyConverter, EuropeanBankCurrencyConverter>();
    services.AddTransient(provider => {
      var configuration = provider.GetRequiredService<IOptions<StreamlabsConfiguration>>();
      var handler = MockHttpMessageHandler.Configure()
                                          .MakeStreamlabsClient(configuration.Value)
                                          .MakeFinanceClient();
      return new HttpClient(handler);
    });
    services.AddSingleton<IAccessTokenStore>(_ => MockAccessTokenStore
                                                  .Create(StreamlabsTokenNameRegistry.AccessToken)
                                                  .Add(StreamlabsTokenNameRegistry.RefreshToken)
                                                  .Add(StreamlabsTokenNameRegistry.AutorizationCode)
                                                  .Add("Twitch")
    );
    services.AddSingleton<IEventDtoContainer>(_ => {
      var events = new List<IEventDto> {
        new StreamlabsDonation(),
        new TwitchSubscription(),
        new TwitchBitsCheer(),
        new TwitchFollow()
      };

      return new EventDtoContainer(events);
    });
    services.AddSingleton<IDonationConverter>(provider => {
      var currencyConverter = provider.GetRequiredService<ICurrencyConverter>();
      return new DonationConverter(currencyConverter, Currency.Usd);
    });
    services.AddSingleton<IFileSystem, MockFileSystem>();
    // services.AddSingleton<IFileSystem>(provider => {
    // var applicationConfig = provider.GetRequiredService<IApplicationConfiguration>();
    // var incentiveFilePath = applicationConfig.Storage.IncentiveFilePath;
    // var mockFiles = new Dictionary<string, MockFileData> {
    //   {
    //     incentiveFilePath,
    //     new MockFileData("")
    //   }
    // };
    // return new MockFileSystem(mockFiles);
    // });
  }
}