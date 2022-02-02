namespace Core.Finance;

public interface ICurrencyConverter {
  public Task<decimal> Convert(decimal amount, ICurrency from, ICurrency to);
}