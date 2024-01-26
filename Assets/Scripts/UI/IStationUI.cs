public interface IStationUI<T>
{
    void Startup(T data);
    ActionMap PreferredActionMap()
    {
        return ActionMap.UI;
    }
}
