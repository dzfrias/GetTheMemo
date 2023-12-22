using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PaperShredderUI : TaskUI
{   
    [SerializeField] private Button shredBtn;
    [SerializeField] private Button saveBtn;
    [SerializeField] private TMP_Text pointsText;
    [SerializeField] private TMP_Text percentageCompleteText;
    [SerializeField] private List<PaperUI> paperUIList;
    [SerializeField] private TaskCheckboxUI taskCheckboxUI;

    private int points = 0;
    private int currentPaperIndex = 0;

    private void Start()
    {
        foreach (PaperUI paperUI in paperUIList)
        {
            paperUI.gameObject.SetActive(false);
        }
        paperUIList[0].gameObject.SetActive(true);

        shredBtn.onClick.AddListener(() => CheckPaper(false));
        saveBtn.onClick.AddListener(() => CheckPaper(true));
    }

    private void CheckPaper(bool playerKeep)
    {
        if (currentPaperIndex == paperUIList.Count) return;

        PaperUI currentPaperUI = paperUIList[currentPaperIndex];
        currentPaperUI.gameObject.SetActive(false);

        currentPaperIndex += 1;
        if (currentPaperIndex != paperUIList.Count)
        {
            PaperUI nextPaper = paperUIList[currentPaperIndex];
            nextPaper.gameObject.SetActive(true);
        }
        else
        {
            taskCheckboxUI.CompleteTask();
        }

        AdjustPoints(currentPaperUI, playerKeep);
        UpdatePercentageText();
    }

    private void UpdatePercentageText()
    {
        float percentageComplete = (float) currentPaperIndex/paperUIList.Count * 100;
        percentageCompleteText.text = $"{percentageComplete:F2}% Complete";
    }

    private void AdjustPoints(PaperUI paperUI, bool playerKeep)
    {
        if (paperUI.KeepPaper())
        {
            if (playerKeep) { points += 1; } 
            else { points -= 1; }
        }
        else
        {
            if (!playerKeep) { points += 1; } 
            else { points -= 1; }
        }
        pointsText.text = $"Points: {points}";
    }
}
