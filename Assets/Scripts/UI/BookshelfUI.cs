using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;
using UnityEngine;

public class BookshelfUI : MonoBehaviour, IStationUI<BookshelfTask>
{
    private BookshelfTask task;

    public void Startup(BookshelfTask task)
    {
        Debug.Log("startup bookshelf ui");
        this.task = task;
    }
}
