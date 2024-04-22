using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargesUI : MonoBehaviour
{
    [SerializeField] private GameObject chargePrefab;
    [SerializeField] private Color chargedColor;
    [SerializeField] private Color inactiveColor;
    [SerializeField] private float spacing = 40f;

    private void OnEnable()
    {
        PlayerCharges.OnMaxChargesChanged += SetCharges;
        PlayerCharges.OnChargesChanged += ActivateCharges;
    }

    private void OnDisable()
    {
        PlayerCharges.OnMaxChargesChanged -= SetCharges;
        PlayerCharges.OnChargesChanged -= ActivateCharges;
    }

    private void SetCharges(int max)
    {
        for (int i = 0; i < max; i++)
        {
            Instantiate(chargePrefab, transform);
            transform.GetChild(i).localPosition = new Vector3(i * spacing, 0, 0);
        }
    }

    private void ActivateCharges(int numCharges)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var childImg = transform.GetChild(i).gameObject.GetComponent<Image>();
            childImg.color = i < numCharges ? chargedColor : inactiveColor;
        }
    }
}
