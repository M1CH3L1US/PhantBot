using Core.Finance;

namespace Core.Interfaces;

public interface IDonation {
  public Task<decimal> ConvertToCurrency(IDonationConverter converter);
}