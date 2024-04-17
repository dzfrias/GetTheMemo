using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventProxy : MonoBehaviour
{
    public Action OnNormalAttack;
    public Action OnSuperAttack;

    public void NormalAttackAnimationEvent()
    {
        OnNormalAttack?.Invoke();
    }

    public void SuperAttackAnimationEvent()
    {
        OnSuperAttack?.Invoke();
    }
}
