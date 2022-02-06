using Core.Streamlabs;
using Infrastructure.Shared.Typing;
using Infrastructure.Streamlabs;
using Xunit;

namespace Infrastructure.Tests.Streamlabs;

public class StreamlabsEventClientTest {
  private IStreamlabsEventClient _sut;

  public StreamlabsEventClientTest(IEventDtoContainer container, IStreamlabsWebsocketClient client) {
    _sut = new StreamlabsEventClient(client, container);
  }

  [Fact]
  public void Test1() {
    Assert.True(true);
  }
}