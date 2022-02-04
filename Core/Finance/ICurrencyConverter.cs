namespace Core.Finance;

public interface ICurrencyConverter {
  public Task<decimal> Convert(decimal amount, ICurrency from, ICurrency to);
  public Task<decimal> ConvertOrDefault(decimal amount, ICurrency from, ICurrency to, decimal defaultValue);
}

public class CurrencyNotFoundException : Exception {
  public CurrencyNotFoundException(ICurrency currency) : base(
    $"Currency {currency.Symbol} not found in exchange rates.") {
  }
}