using Application.Hubs;
using Core.Entities;
using Core.Finance;
using Core.Interfaces;
using Core.Repositories;
using Core.Streamlabs;
using Microsoft.AspNetCore.SignalR;

namespace Application.Services;

public class HubManagementService : IHostedService {
  private readonly IDonationConverter _donationConverter;
  private readonly IHubContext<DonationIncentiveHub> _donationIncentiveHubContext;
  private readonly IDonationIncentiveRepository _incentiveRepository;
  private readonly IStreamlabsEventClient _streamlabsEventClient;

  public HubManagementService(
    IStreamlabsEventClient streamlabsEventClient,
    IHubContext<DonationIncentiveHub> donationIncentiveHubContext,
    IDonationConverter donationConverter,
    IDonationIncentiveRepository incentiveRepository
  ) {
    _streamlabsEventClient = streamlabsEventClient;
    _donationIncentiveHubContext = donationIncentiveHubContext;
    _donationConverter = donationConverter;
    _incentiveRepository = incentiveRepository;
  }

  public async Task StartAsync(CancellationToken cancellationToken) {
    _streamlabsEventClient.DonationReceived().Subscribe(HandleDonation);
  }

  public async Task StopAsync(CancellationToken cancellationToken) {
  }

  private async void HandleDonation(IDonation donation) {
    var donationAmount = await ConvertDonation(donation);
    var updatedIncentive = await UpdateIncentiveAmount(donationAmount);
    await _donationIncentiveHubContext.Clients.All.SendAsync("update", updatedIncentive);
  }

  private Task<decimal> ConvertDonation(IDonation donation) {
    return donation.ConvertToCurrency(_donationConverter);
  }

  private async Task<IDonationIncentive> UpdateIncentiveAmount(decimal donationAmount) {
    await _incentiveRepository.AddToAmount(donationAmount);
    return await _incentiveRepository.GetOrCreate();
  }
}