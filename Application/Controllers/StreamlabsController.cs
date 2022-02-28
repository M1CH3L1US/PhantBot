using Core.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Application.Controllers;

[Route("api/streamlabs")]
public class StreamlabsController {
  private readonly StreamlabsConfiguration _configuration;

  public StreamlabsController(IOptions<StreamlabsConfiguration> options) {
    _configuration = options.Value;
  }

  [HttpGet("codeurl")]
  public IActionResult GetOauthUrl() {
    var url = MakeOAuthAuthorizationUrl();
    return new OkObjectResult(new {url});
  }

  private string MakeOAuthAuthorizationUrl() {
    var qs = new QueryString()
             .Add("response_type", "code")
             .Add("client_id", _configuration.ClientId)
             .Add("redirect_uri", _configuration.RedirectUri)
             .Add("scope", "socket.token");

    return $"{_configuration.BaseUri}/authorize{qs}";
  }
}