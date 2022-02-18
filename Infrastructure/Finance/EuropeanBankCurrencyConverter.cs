using System.Xml;
using Core.Finance;

namespace Infrastructure.Finance;

/// Exchange rate converter using the European Central Bank's web service.
/// https://www.ecb.europa.eu/stats/policy_and_exchange_rates/euro_reference_exchange_rates/html/index.en.html#dev
public class EuropeanBankCurrencyConverter : ICurrencyConverter {
  private const string EcbNinetyDayHistoryUrl = "https://www.ecb.europa.eu/stats/eurofxref/eurofxref-hist-90d.xml";
  private readonly HttpClient _httpClient;

  internal XmlElement? ExchangeRates;

  public EuropeanBankCurrencyConverter(HttpClient httpClient) {
    _httpClient = httpClient;
  }


  public async Task<decimal> Convert(decimal amount, ICurrency from, ICurrency to) {
    if (amount <= 0) {
      throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be greater than zero.");
    }

    var fromRate = await GetConversionRateFromEuro(from);
    var toRate = await GetConversionRateFromEuro(to);

    return amount * (toRate / fromRate);
  }

  public async Task<decimal> ConvertOrDefault(decimal amount, ICurrency from, ICurrency to, decimal defaultValue) {
    try {
      return await Convert(amount, from, to);
    }
    catch (Exception) {
      return defaultValue;
    }
  }

  internal async Task<decimal> GetConversionRateFromEuro(ICurrency currency) {
    if (ExchangeRates is null) {
      await FetchExchangeRates();
    }

    if (currency.Symbol == "EUR") {
      return 1;
    }

    var currencyNode = ExchangeRates!.SelectSingleNode($"//*[@currency='{currency.Symbol}']");

    if (currencyNode is null) {
      throw new CurrencyNotFoundException(currency);
    }

    var rate = currencyNode!.Attributes!["rate"]!.Value;
    return decimal.Parse(rate);
  }

  internal async Task FetchExchangeRates() {
    var document = await GetXmlDocument();

    //// Selects the inner node of the first <Cube> element
    //// <gesmes:Envelope>
    ////  <gesmes:subject>Reference rates</gesmes:subject>
    ////    <gesmes:Sender>
    ////    </gesmes:Sender>
    ////  <Cube>
    ////    <Cube time="2022-01-31">
    ////      ...
    var exchangeRateNode = document["gesmes:Envelope"]!["Cube"]!["Cube"]!;
    //// <Cube currency="USD" rate="1.098"/>
    ExchangeRates = exchangeRateNode;
  }

  internal async Task<XmlDocument> GetXmlDocument() {
    var response = await _httpClient.GetAsync(EcbNinetyDayHistoryUrl);
    var xml = await response.Content.ReadAsStringAsync();
    var document = new XmlDocument();
    document.LoadXml(xml);

    return document;
  }
}