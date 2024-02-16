using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bookshelf : MonoBehaviour, IInteractable
{
    private BookshelfData bookshelfData;

    private void Start()
    {
        bookshelfData = new BookshelfData();
    }

    public void Interact(Vector3 playerPosition)
    {
        StationUIManager.Instance.Startup(bookshelfData);
    }
}
