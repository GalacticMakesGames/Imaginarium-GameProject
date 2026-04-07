using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public event Action OnDamaged;
    public event Action OnDeath;
    
    public float health;
    public float maxHealth;

    private void Start()
    {
        health = maxHealth;
    }

    public void ChangeHealth(float amount)
    {
        health += amount;

        if (health > maxHealth)
            health = maxHealth;

        else if (health <= 0)
            OnDeath?.Invoke(); // check for listeners before taking action

        else if (amount < 0)
            OnDamaged?.Invoke(); // check for listeners before taking action
    }

    //public void ChangeHealth(float amount)
    //{
    //    health -= amount;
    //    OnDamaged?. Invoke();

    //    if (health <= 0)
    //    {
    //        health = 0;
    //        Debug.Log("Died");
    //        OnDeath?.Invoke();
    //    }
}
