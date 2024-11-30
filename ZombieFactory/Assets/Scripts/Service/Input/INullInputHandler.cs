public interface INullInputHandler
{
    void AddEvent(IInputable.Type type, BaseCommand command);
    void RemoveEvent(IInputable.Type type);
}