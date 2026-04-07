using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Rift Mend Spell")]
public class RiftMendSpellSO : SpellSO
{
    [Header("Rift Mend Settings")]
    public float radius = 5;
    public GameObject riftMendFXPrefab;
    public LayerMask openRiftLayer;

    public override void Cast(PlayerController player)
    {

    }
}
