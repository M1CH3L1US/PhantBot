using Application.Hubs;
using Core.Authentication;
using Core.Streamlabs;
using Microsoft.AspNetCore.SignalR;

namespace Application.Services;

public class WebsocketManagementService : IHostedService {
  private readonly IAccessTokenStore _codeStore;
  private readonly IHubContext<DonationIncentiveHub> _donationIncentiveHubContext;
  private readonly IStreamlabsEventClient _streamlabsEventClient;

  public WebsocketManagementService(
    IAccessTokenStore codeStore,
    IStreamlabsEventClient streamlabsEventClient
  ) {
    _codeStore = codeStore;
    _streamlabsEventClient = streamlabsEventClient;
  }

  public async Task StartAsync(CancellationToken cancellationToken) {
    if (_codeStore.HasToken("streamlabs_access_token")) {
      await ConnectToStreamlabs();
    }
  }

  public Task StopAsync(CancellationToken cancellationToken) {
    return UnsubscribeFromEvents();
  }

  private Task UnsubscribeFromEvents() {
    if (_streamlabsEventClient is not null) {
      return _streamlabsEventClient.UnsubscribeFromEvents();
    }

    return Task.CompletedTask;
  }

  public Task ConnectToStreamlabs() {
    Console.WriteLine("Connecting to Streamlabs...");
    return _streamlabsEventClient.SubscribeToEvents();
  }
}