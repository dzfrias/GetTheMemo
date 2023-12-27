using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PaperShredderUI : MonoBehaviour, IStationUI<PaperShredderTask>
{
    [SerializeField]
    private Button shredBtn;

    [SerializeField]
    private Button saveBtn;

    [SerializeField]
    private TMP_Text pointsText;

    [SerializeField]
    private TMP_Text percentageCompleteText;

    [SerializeField]
    private TMP_Text paperText;

    private PaperShredderTask task;

    public void Startup(PaperShredderTask task)
    {
        Debug.Log("PaperShredderUI Startup");
        this.task = task;
    }

    private void Start()
    {
        shredBtn.onClick.AddListener(() => CheckPaper(false));
        saveBtn.onClick.AddListener(() => CheckPaper(true));
        UpdatePaperText();
        UpdatePercentageText();
    }

    private void CheckPaper(bool playerKeep)
    {
        if (task.GetPaperCount() == 0)
            return;

        task.PopPaper();
        task.AdjustPoints(playerKeep);
        TaskManager.Instance.QueueTaskUpdate(task.GetId());
        if (task.GetPaperCount() == 0)
        {
            TaskManager.Instance.CompleteTask(task.GetId());
        }

        UpdatePaperText();
        UpdatePointsText();
        UpdatePercentageText();
    }

    private void UpdatePercentageText()
    {
        int initial = task.GetInitialPaperCount();
        float percentageComplete = (float)(initial - task.GetPaperCount()) / initial * 100;
        percentageCompleteText.text = $"{percentageComplete:F2}% Complete";
    }

    private void UpdatePaperText()
    {
        if (task.GetPaperCount() == 0)
        {
            paperText.text = "";
            return;
        }

        Paper currentPaper = task.GetPaper();
        paperText.text = currentPaper.ShouldKeep() ? "Keep" : "Shred";
    }

    private void UpdatePointsText()
    {
        pointsText.text = $"Points: {task.GetPoints()}";
    }
}
