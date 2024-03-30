using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MoreMountains.Feedbacks;

public class TutorialText : MonoBehaviour
{
    private TextMeshProUGUI text;
    private MMF_Player player;

    private void OnEnable()
    {
        text = GetComponent<TextMeshProUGUI>();
        player = GetComponent<MMF_Player>();
    }

    public void Display(string toDisplay)
    {
        gameObject.SetActive(true);
        text.text = toDisplay;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        player.StopFeedbacks();
    }
}
