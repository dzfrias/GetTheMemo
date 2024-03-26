using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public enum AbilityTier
{
    Tier1,
    Tier2,
    Tier3,
}

public enum AbilityKind
{
    AbilityTest,
}

public interface IAbility
{
    string Name();
    string Description();
    int Cost();
}

public class AbilityTest : IAbility
{
    public string Name()
    {
        return "The nice";
    }

    public string Description()
    {
        return "The ice";
    }

    public int Cost()
    {
        return 10;
    }
}

public class VendingMachineData
{
    public event Action OnShutdown;

    private static AbilityKind[] tier1 = new AbilityKind[] { AbilityKind.AbilityTest };
    private static AbilityKind[] tier2 = new AbilityKind[] { AbilityKind.AbilityTest };
    private static AbilityKind[] tier3 = new AbilityKind[] { AbilityKind.AbilityTest };
    private List<List<IAbility>> abilities;

    public VendingMachineData()
    {
        abilities = new List<List<IAbility>>();
        abilities.Add(NewAbilitySet(AbilityTier.Tier1));
        abilities.Add(NewAbilitySet(AbilityTier.Tier2));
        abilities.Add(NewAbilitySet(AbilityTier.Tier3));
    }

    public List<IAbility> GetTier(AbilityTier tier)
    {
        return abilities[(int)tier];
    }

    public void Shutdown()
    {
        OnShutdown?.Invoke();
    }

    private List<IAbility> NewAbilitySet(AbilityTier tier)
    {
        List<IAbility> set = new List<IAbility>();
        set.Add(NewAbility(tier));
        set.Add(NewAbility(tier));
        set.Add(NewAbility(tier));
        return set;
    }

    private IAbility NewAbility(AbilityTier tier)
    {
        AbilityKind[] abilityPool;
        switch (tier)
        {
            case AbilityTier.Tier1:
                abilityPool = tier1;
                break;
            case AbilityTier.Tier2:
                abilityPool = tier2;
                break;
            case AbilityTier.Tier3:
                abilityPool = tier3;
                break;
            default:
                abilityPool = null;
                break;
        }
        System.Random random = new System.Random();
        int index = random.Next(0, abilityPool.Length);
        AbilityKind kind = abilityPool[index];
        switch (kind)
        {
            case AbilityKind.AbilityTest:
                return new AbilityTest();
            default:
                return null;
        }
    }
}

public class VendingMachine : MonoBehaviour, IInteractable
{
    [SerializeField] private CinemachineVirtualCamera cam;

    private VendingMachineData data;

    private void Start()
    {
        data = new VendingMachineData();
        data.OnShutdown += Shutdown;
    }

    public void Interact(Vector3 _pos)
    {
        cam.gameObject.SetActive(true);
        StationUIManager.Instance.Startup(data);
    }

    private void Shutdown()
    {
        cam.gameObject.SetActive(false);
    }
}
