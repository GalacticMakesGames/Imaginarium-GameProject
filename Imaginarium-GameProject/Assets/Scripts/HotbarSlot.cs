using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class HotbarSlot : MonoBehaviour
{
    //========= ITEM DATA =========//
    public string itemName;
    public int quantity;
    public Sprite itemSprite;
    public bool isFull;
    public string itemDescription;
    public Sprite emptySprite;
    public bool isEquipped;

    //========= ITEM SLOT =========//
    [SerializeField]
    private Image itemImage;

    //========= ITEM DESCRIPTION SLOT =========//
    public Image itemDescriptionImage;
    public TMP_Text itemDescriptionNameText;
    public TMP_Text itemDescriptionText;

    public HotbarManager hotbarManager;

    private void Start()
    {
        hotbarManager = GameObject.Find("Hotbar").GetComponent<HotbarManager>();
    }

    public void AddItem(string itemName, int quantity, Sprite itemSprite, string itemDescription)
    {
        this.itemName = itemName;
        this.quantity = quantity;
        this.itemSprite = itemSprite;
        this.itemDescription = itemDescription;
        isFull = true;

        itemImage.sprite = itemSprite;
    }
}
