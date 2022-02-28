using Application.Hubs;
using Core.Entities;
using Core.Repositories;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Application.Controllers;

[Route("api/incentive")]
public class DonationIncentiveController {
  private readonly IHubContext<DonationIncentiveHub> _hub;
  private readonly IDonationIncentiveRepository _incentiveRepository;

  public DonationIncentiveController(
    IDonationIncentiveRepository incentiveRepository,
    IHubContext<DonationIncentiveHub> hub
  ) {
    _incentiveRepository = incentiveRepository;
    _hub = hub;
  }

  [HttpGet]
  public Task<IDonationIncentive> GetIncentive() {
    return _incentiveRepository.GetOrCreate();
  }

  [HttpPost]
  public async Task UpdateIncentive([FromBody] DonationIncentive incentive) {
    await _incentiveRepository.Set(incentive);
    await SendUpdateToAllClients();
  }

  private async Task SendUpdateToAllClients() {
    var incentive = await _incentiveRepository.GetOrCreate();
    await _hub.Clients.All.SendAsync("update", incentive);
  }
}