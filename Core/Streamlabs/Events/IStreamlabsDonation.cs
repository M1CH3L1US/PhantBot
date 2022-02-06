using Core.Interfaces;

namespace Core.Streamlabs.Events;

public interface IStreamlabsDonation : IDonation {
  public string Name { get; protected set; }
  public string Message { get; protected set; }
  public string UserId { get; protected set; }
  public string Currency { get; protected set; }
  public decimal Amount { get; protected set; }
  public string FormattedAmount { get; protected set; }
  public string FromName { get; protected set; }
}