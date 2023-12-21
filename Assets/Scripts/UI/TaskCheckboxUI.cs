using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskCheckboxUI : MonoBehaviour
{
    [SerializeField] private GameObject checkmarkImage;

    public void CompleteTask()
    {
        checkmarkImage.SetActive(true);
    }
}
