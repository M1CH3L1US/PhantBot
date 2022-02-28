using Core.Authentication;
using Core.Streamlabs;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers;

[Route("api/registration")]
public class ApiRegistrationController {
  private readonly IAccessTokenStore _accessTokenStore;

  private readonly Dictionary<string, string[]> _tokenCategoryMap = new() {
    {"streamlabs", new[] {StreamlabsTokenNameRegistry.AccessToken, StreamlabsTokenNameRegistry.RefreshToken}}
  };

  public ApiRegistrationController(IAccessTokenStore accessTokenStore) {
    _accessTokenStore = accessTokenStore;
  }

  [HttpGet("status")]
  public IActionResult HasAuthenticationCode([FromQuery] string category) {
    _tokenCategoryMap.TryGetValue(category, out var tokenNames);

    if (tokenNames is null) {
      return new OkObjectResult(new {
        isRegistered = false
      });
    }

    var hasAll = tokenNames.All(tokenName => _accessTokenStore.HasToken(tokenName));
    return new OkObjectResult(new {
      isRegistered = hasAll
    });
  }
}