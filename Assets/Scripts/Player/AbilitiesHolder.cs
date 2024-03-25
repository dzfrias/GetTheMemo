using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitiesHolder : MonoBehaviour
{
    [SerializeField] private int balance = 100;

    private List<IAbility> abilities;

    private void Start()
    {
        abilities = new List<IAbility>();
    }

    public void Add(IAbility ability)
    {
        abilities.Add(ability);
        balance -= ability.Cost();
        Debug.Log($"Ability added: {ability.Name()}. New balance: {balance}");
    }

    public IEnumerable<T> GetAbilities<T>()
    {
        return abilities.OfType<T>();
    }

    public T GetAbility<T>()
    {
        return GetAbilities<T>().First();
    }
}
