using Application.Configuration;
using Application.Services;
using Core.Authentication;
using Core.Streamlabs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Application.Controllers;

[Route("api/oauth")]
public class OAuthController {
  private readonly IAccessTokenStore _codeStore;
  private readonly FrontEndConfiguration _frontEndConfiguration;
  private readonly WebsocketManagementService _websocketManagementService;

  public OAuthController(IAccessTokenStore codeStore, WebsocketManagementService websocketManagementService,
    IOptions<FrontEndConfiguration> frontEndConfiguration) {
    _codeStore = codeStore;
    _websocketManagementService = websocketManagementService;
    _frontEndConfiguration = frontEndConfiguration.Value;
  }

  [HttpGet("authorize/callback/streamlabs")]
  public async Task<RedirectResult> AuthorizeStreamlabsCallback([FromQuery(Name = "code")] string code) {
    _codeStore.SetToken(StreamlabsTokenNameRegistry.AutorizationCode, code);

    await _websocketManagementService.ConnectToStreamlabs();

    return new RedirectResult(_frontEndConfiguration.FrontEndUrl);
  }
}