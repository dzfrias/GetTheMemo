using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOnRecord : MonoBehaviour, IRecordMode
{
    public void OnEnterRecordMode()
    {
        gameObject.SetActive(false);
    }
}
