namespace Core.Interfaces;

public interface IDonation {
  public double Value { get; protected set; }
  public string GetValueString();
}