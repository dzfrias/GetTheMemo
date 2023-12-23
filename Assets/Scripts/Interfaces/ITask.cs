public interface ITask
{
    void Start(int id);
    void Complete();
    string Name();
}
