using Core.Repositories;
using Microsoft.AspNetCore.SignalR;

namespace Application.Hubs;

public class DonationIncentiveHub : Hub {
  private readonly IDonationIncentiveRepository _incentiveRepository;

  public DonationIncentiveHub(
    IDonationIncentiveRepository incentiveRepository
  ) {
    _incentiveRepository = incentiveRepository;
  }

  public override async Task OnConnectedAsync() {
    var incentive = await _incentiveRepository.GetOrCreate();
    await Clients.Caller.SendAsync("update", incentive);
  }
}