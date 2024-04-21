using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventProxy : MonoBehaviour
{
    public Action OnPrimaryAttack;
    public Action OnSecondaryAttack;

    public void PrimaryAttackAnimationEvent()
    {
        OnPrimaryAttack?.Invoke();
    }

    public void SecondaryAttackAnimationEvent()
    {
        OnSecondaryAttack?.Invoke();
    }
}
