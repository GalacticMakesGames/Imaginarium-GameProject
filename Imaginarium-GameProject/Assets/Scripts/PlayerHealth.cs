using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public event Action OnPlayerDamaged;
    public event Action OnPlayerDeath;

    public GameOverScript gameOverScript;

    public float playerHealth;
    public float maxPlayerHealth;

    private void Start()
    {
        playerHealth = maxPlayerHealth;
    }

    public void TakeDamage(float amount)
    {
        playerHealth -= amount;
        OnPlayerDamaged?.Invoke();

        if (playerHealth <= 0)
        {
            playerHealth = 0;
            Debug.Log("Died");
            OnPlayerDeath?.Invoke();

            gameOverScript.GameOver();
        }
    }
}
