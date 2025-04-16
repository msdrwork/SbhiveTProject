public interface IEventObserver
{
    void OnEvent(EventId eventId, object payload);
}