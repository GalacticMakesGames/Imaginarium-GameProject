using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    //========= ITEM DATA =========//
    public string itemName;
    public int quantity;
    public Sprite itemSprite;
    public bool isFull;

    //========= ITEM SLOT =========//
    [SerializeField]
    private Image itemImage;

    public void AddItem(string itemName, int quantity, Sprite itemSprite)
    {
        this.itemName = itemName;
        this.quantity = quantity;
        this.itemSprite = itemSprite;
        isFull = true;

        itemImage.sprite = itemSprite;
    }
}
