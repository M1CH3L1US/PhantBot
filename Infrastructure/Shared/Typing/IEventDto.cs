namespace Infrastructure.Shared.Typing;

public interface IEventDto {
  public string EventName { get; }

  public bool MatchDtoName(string dtoName) {
    return EventName.Equals(dtoName);
  }
}