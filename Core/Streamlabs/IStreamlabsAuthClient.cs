using Core.Authentication;

namespace Core.Streamlabs;

public interface IStreamlabsAuthClient : IAuthClient {
  public Task<string> GetSocketAuthToken();
}