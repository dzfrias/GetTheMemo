using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBalance : MonoBehaviour
{
    private float balance;

    private void Start()
    {
        balance = SaveData.Instance.data.playerBalance;
    }

    public void RemoveFromBalance(float amount)
    {
        balance -= amount;
    }

    public float GetBalance()
    {
        return balance;
    }
}
