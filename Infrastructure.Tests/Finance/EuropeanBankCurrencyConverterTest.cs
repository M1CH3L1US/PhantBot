using System;
using System.Net.Http;
using System.Threading.Tasks;
using Core.Finance;
using Core.Interfaces;
using FluentAssertions;
using Infrastructure.Finance;
using Infrastructure.Test.Utils;
using Moq;
using Xunit;

namespace Infrastructure.Test.Finance;

public class EuropeanBankCurrencyConverterTest {
  private readonly EuropeanBankCurrencyConverter _sut;

  public EuropeanBankCurrencyConverterTest() {
    var mockData = FileHelper.GetFileContent("./Finance/TestData/ebc-exchange-rates.xml");
    var mockHttpClient = new Mock<IHttpClient>();

    mockHttpClient.Setup(client => client.GetAsync(It.IsAny<string>())).ReturnsAsync(new HttpResponseMessage {
      Content = new StringContent(mockData)
    });

    _sut = new EuropeanBankCurrencyConverter(mockHttpClient.Object);
  }

  [Fact]
  public async Task FetchExchangeRates_CachesDataInInstance_WhenCalled() {
    await _sut.FetchExchangeRates();
    Assert.NotEmpty(_sut.ExchangeRates);
  }

  [Theory]
  [InlineData(1, "USD", "USD", 1)]
  [InlineData(20, "USD", "JPY", 2308.89)]
  [InlineData(100, "EUR", "CHF", 104.04)]
  public async Task Convert_PerformsAccurateCurrencyConversion_WhenCalled(
    decimal amount,
    string fromSymbol,
    string toSymbol,
    decimal expected
  ) {
    var from = new Currency(fromSymbol, "FromCurrency");
    var to = new Currency(toSymbol, "ToCurrency");

    var convertedValue = await _sut.Convert(amount, from, to);

    Math.Round(convertedValue, 2).Should().Be(expected);
  }

  [Fact]
  public async Task Convert_ThrowsException_WhenCalledWithInvalidCurrency() {
    var from = new Currency("FOO", "FromCurrency");
    var to = new Currency("BAR", "ToCurrency");

    Func<Task> action = async () => await _sut.Convert(1, from, to);

    await action.Should().ThrowAsync<CurrencyNotFoundException>();
  }

  [Fact]
  public async Task Convert_ThrowsException_WhenCalledWithInvalidAmount() {
    var from = new Currency("USD", "FromCurrency");
    var to = new Currency("JPY", "ToCurrency");

    Func<Task> action = async () => await _sut.Convert(-1, from, to);

    await action.Should().ThrowAsync<ArgumentOutOfRangeException>();
  }

  [Fact]
  public async Task ConvertOrDefault_DoesNotThrowException_WhenCalledWithInvalidCurrency() {
    var from = new Currency("FOO", "FromCurrency");
    var to = new Currency("BAR", "ToCurrency");

    var convertedValue = await _sut.ConvertOrDefault(1, from, to, 0);

    convertedValue.Should().Be(0);
  }
}