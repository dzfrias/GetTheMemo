using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookshelfUI : MonoBehaviour, IStationUI<BookshelfData>
{
    private BookshelfData task;

    public void Startup(BookshelfData task)
    {
        Debug.Log("startup bookshelf ui");
        this.task = task;
    }
}
