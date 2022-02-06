namespace Infrastructure.Shared.Typing;

public class EventDtoContainer : IEventDtoContainer {
  private readonly IEnumerable<IEventDto> _dtoTypes;

  public EventDtoContainer(IEnumerable<IEventDto> types) {
    _dtoTypes = types.ToList();
  }

  public IEventDto? GetDtoFromEventName(string eventName) {
    return _dtoTypes.FirstOrDefault(x => x.MatchDtoName(eventName));
  }
}