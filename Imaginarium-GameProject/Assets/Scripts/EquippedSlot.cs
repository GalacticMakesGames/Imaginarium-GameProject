using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
//using static UnityEngine.Rendering.PostProcessing.SubpixelMorphologicalAntialiasing;

public class EquippedSlot : MonoBehaviour
{
    //========= SLOT APPEARANCE =========//
    [SerializeField]
    private Image slotImage;

    //========= SLOT DATA =========//
    private Sprite itemSprite;
    private string itemName;
    private string itemDescription;

    //========= EQUIPPED SLOT =========//
    [SerializeField]
    private EquippedSlot equippedSlot;

    //public Button equipButton;

    //========= OTHER VARIABLES =========//
    public bool slotInUse = false;

    public Button equipButton;

    public ItemSlot[] itemSlot;

    public void OnEquipButtonPress()
    {
        EquipAbility(itemSprite, itemName, itemDescription);
    }

    public void EquipAbility(Sprite itemSprite, string itemName, string itemDescription)
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            if (itemSlot[i].itemDescriptionImage.sprite != null && slotInUse != true)
            {
                // Update Image
                this.itemSprite = itemSprite;
                slotImage.sprite = this.itemSprite;

                // Update Data
                this.itemName = itemName;
                this.itemDescription = itemDescription;

                slotInUse = true;
                return;
            }
        }
        return;
    }
}
