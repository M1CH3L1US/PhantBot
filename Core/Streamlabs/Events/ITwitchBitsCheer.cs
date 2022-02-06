using Core.Interfaces;

namespace Core.Streamlabs.Events;

public interface ITwitchBitsCheer : IDonation {
  string Message { get; protected set; }
  public int Amount { get; protected set; }
  public string Name { get; protected set; }
}