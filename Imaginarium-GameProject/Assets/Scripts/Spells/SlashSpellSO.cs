using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Slash Spell")]
public class SlashSpellSO : SpellSO
{
    [Header("Slash Settings")]
    public int damage = 3;
    public float radius = 5;
    public GameObject slashFXPrefab;
    public LayerMask enemyLayer;

    public override void Cast(PlayerController player)
    {
        
    }
}
