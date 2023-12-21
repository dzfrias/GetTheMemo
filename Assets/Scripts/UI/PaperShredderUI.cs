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

    private void CheckPaper(bool keep)
    {
        if (currentPaperIndex == paperUIList.Count) return;

        PaperUI currentPaperUI = paperUIList[currentPaperIndex];
        if (currentPaperUI.KeepPaper())
        {
            if (keep) { points += 1; } 
            else { points -= 1; }
        }
        else
        {
            if (!keep) { points += 1; } 
            else { points -= 1; }
        }
        currentPaperUI.gameObject.SetActive(false);
        currentPaperIndex += 1;
        if (currentPaperIndex != paperUIList.Count)
        {
            PaperUI nextPaper = paperUIList[currentPaperIndex];
            nextPaper.gameObject.SetActive(true);
        }

        UpdatePercentageText();
        pointsText.text = $"Points: {points}";
    }

    private void UpdatePercentageText()
    {
        float percentageComplete = (float) currentPaperIndex/paperUIList.Count * 100;
        percentageCompleteText.text = $"{percentageComplete:F2}% Complete";
    }
}
