using UnityEngine;

public class PaperShredderTask : ITask
{
    private int id;

    public PaperShredderTask()
    {
        Debug.Log("PaperShredderTask created!");
    }

    public void Start(int id)
    {
        Debug.Log($"PaperShredderTask started with id: {id}!");
        this.id = id;
    }

    public void Complete()
    {
        Debug.Log("PaperShredderTask completed!");
    }

    public string Name()
    {
        return "Shred some paper";
    }

    public int Id()
    {
        return id;
    }
}
