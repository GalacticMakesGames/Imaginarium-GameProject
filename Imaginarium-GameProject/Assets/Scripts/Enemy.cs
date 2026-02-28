using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Health health;
    public GameObject enemy;

    private void OnEnable()
    {
        health.OnDamaged += HandleDamage;
        health.OnDeath += HandleDeath;
    }

    private void OnDisable()
    {
        health.OnDamaged -= HandleDamage;
        health.OnDeath -= HandleDeath;
    }

    void HandleDamage()
    {
        Debug.Log("Enemy Damaged!");
    }

    void HandleDeath()
    {
        Debug.Log("Enemy Died!");
        Destroy(enemy);
    }
}
