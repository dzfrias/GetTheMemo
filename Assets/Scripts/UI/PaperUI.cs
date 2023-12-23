using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PaperUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text paperText;

    [SerializeField]
    private bool keepPaper;

    private void Awake()
    {
        SetPaperText();
    }

    public bool KeepPaper()
    {
        return keepPaper;
    }

    private void SetPaperText()
    {
        if (keepPaper)
        {
            paperText.text = "You should keep this paper.";
        }
        else
        {
            paperText.text = "DESTROY MEEEEE";
        }
    }
}
