using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperShredder : MonoBehaviour, IInteractable
{
    private PaperShredderData paperShredderData;

    private void Start()
    {
        paperShredderData = new PaperShredderData();
    }

    public void Interact(Vector3 playerPosition)
    {
        StationUIManager.Instance.Startup(paperShredderData);
    }
}
