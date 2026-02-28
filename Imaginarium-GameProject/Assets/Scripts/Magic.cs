using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic : MonoBehaviour
{
    public PlayerController playerController;

    [Header("Slash Variables")]
    public int damage = 3;
    public float radius = 5;
    public float spellRange;
    public float spellCooldown;
    public GameObject slashFXPrefab;
    public LayerMask enemyLayer;

    public bool CanCast => Time.time >= nextCastTime;
    private float nextCastTime;

    public void CastSpell()
    {
        Slash();
    }

    private void Slash()
    {
        if (!CanCast)
            return;

        Collider[] enemies = Physics.OverlapSphere(playerController.transform.position, radius, enemyLayer);

        foreach (Collider enemy in enemies)
        {
            Health health = enemy.GetComponent<Health>();
            if(health != null)
            {
                health.ChangeHealth(-damage);
            }

            if(slashFXPrefab != null)
            {
                GameObject newFX = Instantiate(slashFXPrefab, enemy.transform.position, Quaternion.identity);
                Destroy(newFX, 2);
            }
        }

        nextCastTime = Time.time + spellCooldown;
    }
}
