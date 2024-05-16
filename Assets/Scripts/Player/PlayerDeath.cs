using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    [SerializeField] private Transform canvas;
    [SerializeField] private GameObject damageIndicatorObject;
    [SerializeField] private GameObject deathScreenUI;
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject playerCamera;

    private Health health;
    private Collider collider;

    private void Awake()
    {
        health = GetComponent<Health>();
        collider = GetComponent<Collider>();
    }

    private void OnEnable()
    {
        health.OnDamagedTaken += OnDamageTaken;
        health.OnDeath += Health_OnDeath;
    }

    private void OnDisable()
    {
        health.OnDamagedTaken -= OnDamageTaken;
        health.OnDeath -= Health_OnDeath;
    }

    private void Health_OnDeath()
    {
        collider.enabled = false;
        playerCamera.SetActive(false);
        mainCamera.transform.parent.position = playerCamera.transform.position;
        mainCamera.GetComponent<Animator>().SetTrigger("Death");
        deathScreenUI.SetActive(true);
        GameInput.Instance.SwitchActionMaps(ActionMap.UI);
    }

    private void OnDamageTaken(Vector3 location)
    {
        DamageIndicatorUI damageIndicatorUI = Instantiate(damageIndicatorObject, canvas).GetComponent<DamageIndicatorUI>();
        damageIndicatorUI.Setup(location, transform);
    }
}
