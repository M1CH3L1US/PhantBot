using System.Net.Http;
using System.Threading.Tasks;
using Infrastructure.Finance;
using Infrastructure.Test.Utils;
using Moq;
using Xunit;

namespace Infrastructure.Test.Finance;

public class EuropeanBankCurrencyConverterTest {
  private readonly EuropeanBankCurrencyConverter _sut;

  public EuropeanBankCurrencyConverterTest() {
    var mockData = FileHelper.GetFileContent("./Finance/TestData/ebc-exchange-rates.xml");
    var mockHttpClient = new Mock<HttpClient>();

    mockHttpClient.Setup(client => client.GetAsync(It.IsAny<string>())).ReturnsAsync(new HttpResponseMessage {
      Content = new StringContent(mockData)
    });

    _sut = new EuropeanBankCurrencyConverter(mockHttpClient.Object);
  }

  [Fact]
  public async Task FetchExchangeRates_CachesDataInInstance_WhenCalled() {
    await _sut.FetchExchangeRates();
    Assert.NotEmpty(_sut._exchangeRates);
  }
}