using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TaskCheckboxUI : MonoBehaviour
{
    [SerializeField] private GameObject checkmarkImage;
    [SerializeField] private TextMeshProUGUI taskText;

    public void CompleteTask()
    {
        checkmarkImage.SetActive(true);
    }

    public void SetTaskText(string text)
    {
        taskText.text = text;
    }
}
