using Core.Configuration;
using Core.Finance;
using Core.Interfaces;
using Core.Twitch.Http;
using Core.Twitch.Websocket;
using Infrastructure.Finance;
using Infrastructure.Shared.Typing;
using Infrastructure.Twitch;
using Infrastructure.Twitch.Http;
using Infrastructure.Twitch.Websocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class ServiceCollectionExtension {
  public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration) {
    // Allows us to use the internal abstraction rather
    // than the existing concrete class
    // ReSharper disable once SuspiciousTypeConversion.Global
    services.AddSingleton((IHttpClient) new HttpClient());

    services.AddSingleton<ITwitchApiClient, TwitchApiClient>();
    services.AddSingleton<ITwitchHttpClient, TwitchHttpClient>();
    services.Configure<StreamlabsConfiguration>(configuration.GetRequiredSection("Streamlabs"));
    services.Configure<TwitchConfiguration>(configuration.GetRequiredSection("Twitch"));
    services.Configure<StorageConfiguration>(configuration.GetRequiredSection("Storage"));

    services.AddSingleton<ITwitchEventClient, TwitchEventClient>();
    services.AddSingleton<ITwitchWebsocketClient, TwitchWebsocketClient>();
    services.AddSingleton<ICurrencyConverter, EuropeanBankCurrencyConverter>();
    services.AddSingleton<IDonationConverter>(provider => {
      var currencyConverter = provider.GetRequiredService<ICurrencyConverter>();
      return new DonationConverter(currencyConverter, Currency.Usd);
    });
    services.AddSingleton<IEventDtoContainer, EventDtoContainer>();
  }
}