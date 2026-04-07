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

    [Header("Rift Mend Variables")]
    public float riftFixRadius = 5;
    //public GameObject openRift;
    public LayerMask openRiftLayer;

    [Header("Flaming Glory Variables")]
    public float flamingGloryRadius = 5;
    //public GameObject flame;
    public LayerMask towerPuzzleLayer;

    public bool CanCast => Time.time >= nextCastTime;
    private float nextCastTime;

    public void CastSlashSpell()
    {
        Slash();
    }

    public void CastRiftMendSpell()
    {
        RiftMend();
    }

    public void CastFlamingGlorySpell()
    {
        FlamingGlory();
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

    private void RiftMend()
    {
        if (!CanCast)
            return;

        Collider[] openrifts = Physics.OverlapSphere(playerController.transform.position, riftFixRadius, openRiftLayer);

        foreach (Collider openrift in openrifts)
        {
            if (openrift.CompareTag("OpenRift"))
            {
                // Destroy the matching GameObject
                Destroy(openrift.gameObject);
            }
        }

        nextCastTime = Time.time + spellCooldown;
    }

    private void FlamingGlory()
    {
        if (!CanCast)
            return;

        Collider[] towerpillars = Physics.OverlapSphere(playerController.transform.position, flamingGloryRadius, towerPuzzleLayer);

        foreach (Collider towerpillar in towerpillars)
        {
            if (towerpillar.CompareTag("TowerPuzzle"))
            {
                // Try to get the Pillar script attached to the collided object
                Pillar pillarScript = towerpillar.GetComponent<Pillar>();

                // If the script exists and a flame child was assigned in the Inspector
                if (pillarScript != null && pillarScript.flameChild != null)
                {
                    pillarScript.flameChild.SetActive(true);
                }
            }
        }

        nextCastTime = Time.time + spellCooldown;
    }
}
