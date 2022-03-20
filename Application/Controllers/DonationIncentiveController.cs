using Application.Hubs;
using Core.Entities;
using Core.Repositories;
using Core.Streamlabs;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Application.Controllers;

[Route("api/incentive")]
public class DonationIncentiveController {
  private const string BitsTestDonationPayload = @"{
  ""type"": ""bits"",
  ""message"": [
  {
    ""id"": ""fc546f7d-aab8-42b2-9933-8681e9fb5eb0"",
    ""name"": ""h4r5h48002"",
    ""amount"": ""20"",
    ""emotes"": null,
    ""message"": ""streamlabs1"",
    ""_id"": ""74a0b93e736f1f14762111f8ae34bf42""
  }
  ],
  ""for"": ""twitch_account""
}";

  private readonly IHubContext<DonationIncentiveHub> _hub;
  private readonly IDonationIncentiveRepository _incentiveRepository;
  private readonly IStreamlabsEventClient _streamlabsEventClient;

  public DonationIncentiveController(
    IDonationIncentiveRepository incentiveRepository,
    IHubContext<DonationIncentiveHub> hub,
    IStreamlabsEventClient streamlabsEventClient
  ) {
    _incentiveRepository = incentiveRepository;
    _hub = hub;
    _streamlabsEventClient = streamlabsEventClient;
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

  [HttpPost("test")]
  public async Task InvokeStreamlabsUpdate() {
    _streamlabsEventClient.SocketClient.SocketEventReceived.OnNext(BitsTestDonationPayload);
  }
}