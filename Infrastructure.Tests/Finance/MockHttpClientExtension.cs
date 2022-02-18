using Infrastructure.Tests.Mocking.Http;
using Infrastructure.Tests.Utils;

namespace Infrastructure.Tests.Finance;

public static class MockHttpClientExtension {
  private const string ExchangeRatesUrl = "https://www.ecb.europa.eu/stats/eurofxref/eurofxref-hist-90d.xml";

  public static MockHttpMessageHandler MakeFinanceClient(this MockHttpMessageHandler client) {
    var mockData = FileHelper.GetFileContent("./Finance/TestData/ebc-exchange-rates.xml");
    return client.WithGet(ExchangeRatesUrl, mockData);
  }
}