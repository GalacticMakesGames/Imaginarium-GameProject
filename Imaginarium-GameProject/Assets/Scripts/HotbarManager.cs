using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotbarManager : MonoBehaviour
{
    public HotbarSlot[] hotbarSlot;

    public void AddItem(string itemName, int quantity, Sprite itemSprite, string itemDescription)
    {
        //Debug.Log("itemName = " + itemName + "quantity = " + quantity + "itemSprite = " + itemSprite);

        for (int i = 0; i < hotbarSlot.Length; i++)
        {
            if (hotbarSlot[i].isFull == false)
            {
                hotbarSlot[i].AddItem(itemName, quantity, itemSprite, itemDescription);
                return;
            }
        }
        return;
    }

    // This looks through your inventory to see if a specific item is collected
    public bool CheckIfHasItem(string searchName)
    {
        // Check the inventory slots
        for (int i = 0; i < hotbarSlot.Length; i++)
        {
            // If the slot is full, check if its name matches the one we are searching for
            if (hotbarSlot[i].isFull == true && hotbarSlot[i].itemName == searchName)
            {
                return true;
            }
        }

        // If we checked every single slot and found nothing, return false
        return false;
    }
}
