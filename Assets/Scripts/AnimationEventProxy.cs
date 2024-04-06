using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventProxy : MonoBehaviour
{
    public Action OnAttack;

    public void AttackAnimationEvent()
    {
        OnAttack?.Invoke();
    }
}
