using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TutorialWatcher : MonoBehaviour
{
    private enum AdvanceType
    {
        Melee,
        Dash,
        Super,
        None,
    }

    [SerializeField] private DamageBox dashDamageBox;
    
    private PlayerMeleeAttack melee;
    private AdvanceType advanceType = AdvanceType.None;

    private void Awake()
    {
        melee = GetComponent<PlayerMeleeAttack>();
    }

    private void OnEnable()
    {
        melee.OnHit += () => DidPerform(AdvanceType.Melee);
        melee.OnSuperHit += () => DidPerform(AdvanceType.Super);
        dashDamageBox.OnHit += () => DidPerform(AdvanceType.Dash);
    }

    private void DidPerform(AdvanceType toAdvance)
    {
        if (advanceType != toAdvance) return;
        advanceType = AdvanceType.None;
        OfficeManager.Instance.NextBeat();
    }

    public void SetMeleeAdvance()
    {
        advanceType = AdvanceType.Melee;
    }

    public void SetDashAdvance()
    {
        advanceType = AdvanceType.Dash;
    }

    public void SetSuperAdvance()
    {
        advanceType = AdvanceType.Super;
    }
}
