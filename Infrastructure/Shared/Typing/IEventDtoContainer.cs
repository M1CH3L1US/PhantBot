namespace Infrastructure.Shared.Typing;

public interface IEventDtoContainer {
  public IEventDto? GetDtoFromEventName(string eventName);
}