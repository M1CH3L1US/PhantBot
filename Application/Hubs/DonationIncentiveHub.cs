using Core.Entities;
using Core.Finance;
using Core.Interfaces;
using Core.Repositories;
using Core.Streamlabs;
using Microsoft.AspNetCore.SignalR;

namespace Application.Hubs;

public class DonationIncentiveHub : Hub {
  private readonly IDonationConverter _donationConverter;
  private readonly IDonationIncentiveRepository _incentiveRepository;
  private readonly IStreamlabsEventClient _streamlabsEventClient;

  public DonationIncentiveHub(
    IStreamlabsEventClient streamlabsEventClient,
    IDonationIncentiveRepository incentiveRepository,
    IDonationConverter donationConverter
  ) {
    _streamlabsEventClient = streamlabsEventClient;
    _incentiveRepository = incentiveRepository;
    _donationConverter = donationConverter;
    ListenToDonations();
  }

  private void ListenToDonations() {
    _streamlabsEventClient.DonationReceived()
                          .Subscribe(donation => HandleDonation(donation));
  }

  private async Task HandleDonation(IDonation donation) {
    var donationAmount = await ConvertDonation(donation);
    var updatedIncentive = UpdateIncentiveAmount(donationAmount);
    await Clients.All.SendAsync("update", updatedIncentive);
  }

  private Task<decimal> ConvertDonation(IDonation donation) {
    return donation.ConvertToCurrency(_donationConverter);
  }

  private async Task<IDonationIncentive> UpdateIncentiveAmount(decimal donationAmount) {
    await _incentiveRepository.AddToAmount(donationAmount);
    return await _incentiveRepository.GetOrCreate();
  }

  public override async Task OnConnectedAsync() {
    var incentive = await _incentiveRepository.GetOrCreate();
    await Clients.Caller.SendAsync("update", incentive);
  }
}