using System.Xml;
using Core.Finance;

namespace Infrastructure.Finance;

public class EuropeanBankCurrencyConverter : ICurrencyConverter {
  private const string EcbNinetyDayHistoryUrl = "https://www.ecb.europa.eu/stats/eurofxref/eurofxref-hist-90d.xml";
  private readonly HttpClient _httpClient;

  internal XmlElement? _exchangeRates;

  public EuropeanBankCurrencyConverter(HttpClient httpClient) {
    _httpClient = httpClient;
  }

  public async Task<decimal> Convert(decimal amount, ICurrency from, ICurrency to) {
    var fromRate = await GetRate(from);
    var toRate = await GetRate(to);

    return amount * (toRate / fromRate);
  }

  internal async Task<decimal> GetRate(ICurrency currency) {
    if (_exchangeRates is null) {
      await FetchExchangeRates();
    }

    var currencyNode = _exchangeRates!.SelectSingleNode($"//*[@currency='{currency.Symbol}']");
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
    _exchangeRates = exchangeRateNode;
  }

  internal async Task<XmlDocument> GetXmlDocument() {
    var response = await _httpClient.GetAsync(EcbNinetyDayHistoryUrl);
    var xml = await response.Content.ReadAsStringAsync();
    var document = new XmlDocument();
    document.LoadXml(xml);

    return document;
  }
}