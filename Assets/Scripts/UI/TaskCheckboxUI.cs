using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TaskCheckboxUI : MonoBehaviour
{
    [SerializeField]
    private GameObject checkmarkImage;

    [SerializeField]
    private TextMeshProUGUI taskText;

    public void CompleteTask()
    {
        checkmarkImage.SetActive(true);
    }

    public void SetTaskText(string text)
    {
        taskText.text = text;
    }
}
