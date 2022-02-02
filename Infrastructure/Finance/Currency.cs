using Core.Finance;

namespace Infrastructure.Finance;

public class Currency : ICurrency {
  public Currency(string name, string symbol) {
    Name = name;
    Symbol = symbol;
  }

  public static ICurrency Jpy { get; } = new Currency("JPY", nameof(Jpy));
  public static ICurrency Usd { get; } = new Currency("USD", nameof(Usd));
  public static ICurrency Bgn { get; } = new Currency("BGN", nameof(Bgn));
  public static ICurrency Czk { get; } = new Currency("CZK", nameof(Czk));
  public static ICurrency Dkk { get; } = new Currency("DKK", nameof(Dkk));
  public static ICurrency Gbp { get; } = new Currency("GBP", nameof(Gbp));
  public static ICurrency Huf { get; } = new Currency("HUF", nameof(Huf));
  public static ICurrency Pln { get; } = new Currency("PLN", nameof(Pln));
  public static ICurrency Ron { get; } = new Currency("RON", nameof(Ron));
  public static ICurrency Sek { get; } = new Currency("SEK", nameof(Sek));
  public static ICurrency Chf { get; } = new Currency("CHF", nameof(Chf));
  public static ICurrency Isk { get; } = new Currency("ISK", nameof(Isk));
  public static ICurrency Nok { get; } = new Currency("NOK", nameof(Nok));
  public static ICurrency Hrk { get; } = new Currency("HRK", nameof(Hrk));
  public static ICurrency Rub { get; } = new Currency("RUB", nameof(Rub));
  public static ICurrency Try { get; } = new Currency("TRY", nameof(Try));
  public static ICurrency Aud { get; } = new Currency("AUD", nameof(Aud));
  public static ICurrency Brl { get; } = new Currency("BRL", nameof(Brl));
  public static ICurrency Cad { get; } = new Currency("CAD", nameof(Cad));
  public static ICurrency Cny { get; } = new Currency("CNY", nameof(Cny));
  public static ICurrency Hkd { get; } = new Currency("HKD", nameof(Hkd));
  public static ICurrency Idr { get; } = new Currency("IDR", nameof(Idr));
  public static ICurrency Ils { get; } = new Currency("ILS", nameof(Ils));
  public static ICurrency Inr { get; } = new Currency("INR", nameof(Inr));
  public static ICurrency Krw { get; } = new Currency("KRW", nameof(Krw));
  public static ICurrency Mxn { get; } = new Currency("MXN", nameof(Mxn));
  public static ICurrency Myr { get; } = new Currency("MYR", nameof(Myr));
  public static ICurrency Nzd { get; } = new Currency("NZD", nameof(Nzd));
  public static ICurrency Php { get; } = new Currency("PHP", nameof(Php));
  public static ICurrency Sgd { get; } = new Currency("SGD", nameof(Sgd));
  public static ICurrency Thb { get; } = new Currency("THB", nameof(Thb));
  public static ICurrency Zar { get; } = new Currency("ZAR", nameof(Zar));

  public string Name { get; }
  public string Symbol { get; }
}