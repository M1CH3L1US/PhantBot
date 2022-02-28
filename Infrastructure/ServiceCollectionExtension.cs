using System.IO.Abstractions;
using Core.Authentication;
using Core.Configuration;
using Core.Finance;
using Core.Repositories;
using Core.Storage;
using Core.Streamlabs;
using Infrastructure.Authentication.Stores;
using Infrastructure.Finance;
using Infrastructure.Repositories;
using Infrastructure.Shared.Typing;
using Infrastructure.Storage;
using Infrastructure.Streamlabs;
using Infrastructure.Streamlabs.Socket;
using Infrastructure.Streamlabs.Socket.Dto;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Socket.Io.Client.Core;

namespace Infrastructure;

public static class ServiceCollectionExtension {
  public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration) {
    services.Configure<StreamlabsConfiguration>(configuration.GetRequiredSection("Streamlabs"));
    services.Configure<TwitchConfiguration>(configuration.GetRequiredSection("Twitch"));
    services.Configure<StorageConfiguration>(configuration.GetRequiredSection("Storage"));

    services.AddSingleton(new HttpClient());

    services.AddSingleton<IAuthClient, StreamlabsAuthClient>();
    services.AddSingleton<ISocketIoClient>(_ => {
      var url = new Uri("https://will-be-replaced-by-config.com");
      return new SocketIoClient();
    });
    services.AddSingleton<IAccessTokenStore, EnvironmentAccessTokenStore>();

    services.AddSingleton<IStreamlabsAuthClient, StreamlabsAuthClient>();
    services.AddSingleton<IStreamlabsEventClient, StreamlabsEventClient>();
    services.AddSingleton<IStreamlabsSocketClient, StreamlabsSocketClient>();

    /*
     Not yet implemented
      services.AddSingleton<ITwitchApiClient, TwitchApiClient>();
      services.AddSingleton<ITwitchHttpClient, TwitchHttpClient>();
      services.AddSingleton<ITwitchEventClient, TwitchEventClient>();
      services.AddSingleton<ITwitchWebsocketClient, TwitchWebsocketClient>();
    */

    services.AddSingleton<ICurrencyConverter, EuropeanBankCurrencyConverter>();
    services.AddSingleton<IDonationConverter>(provider => {
      var currencyConverter = provider.GetRequiredService<ICurrencyConverter>();
      return new DonationConverter(currencyConverter, Currency.Usd);
    });

    services.AddSingleton<IDonationIncentiveRepository, DonationIncentiveFileSystemRepository>();
    services.AddSingleton<IThreadSafeFileWriter, ThreadSafeFileWriter>();
    services.AddSingleton<IFileSystem, FileSystem>();

    services.AddSingleton<IEventDtoContainer>(_ => {
      var events = new List<IEventDto> {
        new StreamlabsDonation(),
        new TwitchSubscription(),
        new TwitchBitsCheer(),
        new TwitchFollow()
      };

      return new EventDtoContainer(events);
    });
  }
}