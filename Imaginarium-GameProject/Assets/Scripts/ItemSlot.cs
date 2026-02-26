using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ItemSlot : MonoBehaviour, IPointerClickHandler
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

    public GameObject selectedShader;
    public bool thisItemSelected;

    //========= EQUIPPED SLOT =========//
    [SerializeField]
    private EquippedSlot equippedSlot;

    public Button equipButton;

    public InventoryManager inventoryManager;

    private void Start()
    {
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isFull != false)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                OnLeftClick();
            }
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                OnRightClick();
            }
        }
    }

    public void OnLeftClick()
    {
        inventoryManager.DeselectAllSlots();
        selectedShader.SetActive(true);
        thisItemSelected = true;
        itemDescriptionNameText.text = itemName;
        itemDescriptionText.text = itemDescription;
        itemDescriptionImage.sprite = itemSprite;

        if (itemDescriptionImage.sprite == null)
        {
            itemDescriptionImage.sprite = emptySprite;
        }
    }

    public void OnRightClick()
    {

    }

    public void EquipAbility()
    {
        if (itemDescriptionImage.sprite != null)
        {
            equippedSlot.EquipAbility(itemSprite, itemName, itemDescription);
        }

        // rather than emptying the slot upon equipping that ability, create another shader around the slots that will be turned on when equipped, similiar to selected shader
    }
}
