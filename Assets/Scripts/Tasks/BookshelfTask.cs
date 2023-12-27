using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookshelfTask : ITask
{
    private int id;

    public BookshelfTask()
    {
        Debug.Log("Bookshelf task created!");
    }

    public void Complete()
    {
        Debug.Log("BookselfTask completed!");
    }

    public string GetName()
    {
        return "Sort Books";
    }

    public void Start(int id)
    {
        Debug.Log($"BookshelfTask started with id: {id}!");
        this.id = id;
    }
}
