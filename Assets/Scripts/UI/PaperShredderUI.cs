using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PaperShredderUI : MonoBehaviour, IStationUI<PaperShredderData>
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

    private PaperShredderData data;

    public void Startup(PaperShredderData data)
    {
        Debug.Log("PaperShredderUI Startup");
        this.data = data;
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
        if (data.GetPaperCount() == 0)
            return;

        data.AdjustPoints(playerKeep);
        data.PopPaper();
        if (data.GetPaperCount() == 0)
        {
            Debug.Log("done with paper!");
        }

        UpdatePaperText();
        UpdatePointsText();
        UpdatePercentageText();
    }

    private void UpdatePercentageText()
    {
        int initial = data.GetInitialPaperCount();
        float percentageComplete = (float)(initial - data.GetPaperCount()) / initial * 100;
        percentageCompleteText.text = $"{percentageComplete:F2}% Complete";
    }

    private void UpdatePaperText()
    {
        if (data.GetPaperCount() == 0)
        {
            paperText.text = "";
            return;
        }

        Paper currentPaper = data.GetPaper();
        paperText.text = currentPaper.ShouldKeep() ? "Keep" : "Shred";
    }

    private void UpdatePointsText()
    {
        pointsText.text = $"Points: {data.GetPoints()}";
    }
}
