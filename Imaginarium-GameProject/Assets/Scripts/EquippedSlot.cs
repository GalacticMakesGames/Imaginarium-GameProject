using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquippedSlot : MonoBehaviour
{
    //========= SLOT APPEARANCE =========//
    [SerializeField]
    private Image slotImage;

    //========= SLOT DATA =========//
    private Sprite itemSprite;
    private string itemName;
    private string itemDescription;

    //========= OTHER VARIABLES =========//
    private bool slotInUse = false;

    public void EquipAbility(Sprite itemSprite, string itemName, string itemDescription)
    {
        // Update Image
        this.itemSprite = itemSprite;
        slotImage.sprite = this.itemSprite;

        // Update Data
        this.itemName = itemName;
        this.itemDescription = itemDescription;

        slotInUse = true;
    }
}
