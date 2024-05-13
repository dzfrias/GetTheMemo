using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialLightPanelRow : MonoBehaviour
{
    [SerializeField] private float delay = 0.75f;

    public void Activate()
    {
        StartCoroutine(_Activate());
    }

    private IEnumerator _Activate()
    {
        int i = 0;
        foreach (Transform child in transform)
        {
            // Regular light panel
            if (i % 2 == 0)
            {
                child.gameObject.SetActive(true);
            }
            else
            {
                child.gameObject.SetActive(false);
                yield return new WaitForSeconds(delay);
            }
            i++;
        }
    }
}
