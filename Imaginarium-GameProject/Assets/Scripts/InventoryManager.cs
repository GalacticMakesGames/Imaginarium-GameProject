using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using static UnityEngine.Rendering.PostProcessing.SubpixelMorphologicalAntialiasing;

public class InventoryManager : MonoBehaviour
{
    public GameObject InventoryMenu;
    private bool menuActivated;
    
    public ItemSlot[] itemSlot;
    public EquippedSlot[] equippedSlot;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && menuActivated)
        {
            Time.timeScale = 1;
            InventoryMenu.SetActive(false);
            menuActivated = false;
        }

        else if (Input.GetKeyDown(KeyCode.F) && !menuActivated)
        {
            Time.timeScale = 0;
            InventoryMenu.SetActive(true);
            menuActivated = true;
        }
    }

    public void AddItem(string itemName, int quantity, Sprite itemSprite, string itemDescription)
    {
        //Debug.Log("itemName = " + itemName + "quantity = " + quantity + "itemSprite = " + itemSprite);

        for (int i = 0; i < itemSlot.Length; i++)
        {
            if (itemSlot[i].isFull == false)
            {
                itemSlot[i].AddItem(itemName, quantity, itemSprite, itemDescription);
                return;
            }
        }
        return;
    }

    public void EquipAbility(string itemName, Sprite itemSprite, string itemDescription)
    {
        //Debug.Log("itemName = " + itemName + "quantity = " + quantity + "itemSprite = " + itemSprite);

        for (int i = 0; i < equippedSlot.Length; i++)
        {
            if (equippedSlot[i].slotInUse == false)
            {
                equippedSlot[i].EquipAbility(itemSprite, itemName, itemDescription);
                return;
            }
        }
        return;
    }

    public void DeselectAllSlots()
    {
        for(int i = 0; i < itemSlot.Length; i++)
        {
            itemSlot[i].selectedShader.SetActive(false);
            itemSlot[i].thisItemSelected = false;
        }
    }

    // --- NEW FUNCTION ADDED FOR THE SHAPE RECOGNIZER ---
    // This looks through your inventory to see if a specific item is collected
    public bool CheckIfHasItem(string searchName)
    {
        // 1. Check the regular inventory slots first
        for (int i = 0; i < itemSlot.Length; i++)
        {
            // If the slot is full, check if its name matches the one we are searching for
            if (itemSlot[i].isFull == true && itemSlot[i].itemName == searchName)
            {
                return true; // We found it!
            }
        }

        // If we checked every single slot and found nothing, return false.
        return false;
    }

    //public void Equip()
    //{
    //    Debug.Log("Ability has been equipped!");

    //    for (int i = 0; i < itemSlot.Length; i++)
    //    {
    //        if (itemSlot[i].isEquipped == false)
    //        {
    //            itemSlot[i].itemSprite = itemSlot[i].itemSprite;
    //            return;
    //        }
    //    }
    //}
}
