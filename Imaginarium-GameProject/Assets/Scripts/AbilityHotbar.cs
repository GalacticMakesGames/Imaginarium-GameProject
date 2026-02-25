using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AbilityHotbar : MonoBehaviour
{
    //========= ITEM DATA =========//
    public Sprite itemSprite;
    public Sprite emptySprite;
    public bool isEquipped;

    //========= ITEM SLOT =========//
    [SerializeField]
    private Image itemImage;
    public bool isFull;

    public InventoryManager inventoryManager;

    // Start is called before the first frame update
    private void Start()
    {
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
    }

    public void AddItem(Sprite itemSprite)
    {
        this.itemSprite = itemSprite;
        isFull = true;
        itemImage.sprite = itemSprite;
        isEquipped = true;
    }
}
