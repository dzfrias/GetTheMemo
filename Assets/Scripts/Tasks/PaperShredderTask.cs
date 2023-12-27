using System.Collections.Generic;
using UnityEngine;

public class Paper
{
    private bool shouldKeep;

    public Paper()
    {
        bool randomBool = Random.value > 0.5f;
        shouldKeep = randomBool;
    }

    public bool ShouldKeep()
    {
        return shouldKeep;
    }
}

public class PaperShredderTask : ITask
{
    private int id;
    private List<Paper> papers;
    private int initial;
    private int points;

    public PaperShredderTask(int n = 7)
    {
        papers = new List<Paper>();
        for (int i = 0; i < n; i++)
        {
            papers.Add(new Paper());
        }
        initial = n;
        Debug.Log("PaperShredderTask started!");
    }

    public Paper GetPaper()
    {
        return papers[papers.Count - 1];
    }

    public int GetPaperCount()
    {
        return papers.Count;
    }

    public int GetInitialPaperCount()
    {
        return initial;
    }

    public int GetPoints()
    {
        return points;
    }

    public void PopPaper(bool keep)
    {
        Paper paper = GetPaper();
        papers.RemoveAt(papers.Count - 1);
        int delta = keep == paper.ShouldKeep() ? 1 : -1;
        points += delta;
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

    public string GetName()
    {
        int n = GetPaperCount();
        return $"Shred {n} pieces of paper";
    }

    public int GetId()
    {
        return id;
    }
}
