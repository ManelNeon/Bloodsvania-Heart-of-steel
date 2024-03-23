using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    //code that functions exactly the same as the special code, with the difference that now it contains a image

    // ----- Items Data ----- //

    [HideInInspector] public string itemName;

    [HideInInspector] public int quantity;

    Sprite itemSprite;

    string itemDescription;

    /*
    1 - Healing
    2 - Regening Blood
    3 - Debuffing the enemy
    */
    int itemCode;

    int effectQuantity;

    [HideInInspector] public bool isFull;

    // ---- Item Slot ---- //

    [SerializeField] TMP_Text quantityText;

    [SerializeField] Image itemImage;

    [SerializeField] Transform position;

    // ---- Item Description ---- //

    [SerializeField] GameObject descriptionBox;

    [SerializeField] Image descriptionImage;

    [SerializeField] TextMeshProUGUI descriptionText;

    [SerializeField] TextMeshProUGUI descriptionTitle;

    //adding the item to the slot
    public void AddItem(string itemName, int quantity, Sprite itemSprite, string itemDescription, int itemCode, int effectQuantity)
    {
        this.itemName = itemName;

        this.quantity = quantity;

        this.itemSprite = itemSprite;

        this.itemDescription = itemDescription;

        this.itemCode = itemCode;

        this.effectQuantity = effectQuantity;

        isFull = true;

        quantityText.text = quantity.ToString();
        quantityText.enabled = true;

        itemImage.sprite = itemSprite;
    }

    //changing the quantity
    public void ChangeQuantity()
    {
        quantityText.text = quantity.ToString();
    }

    //applying the description
    void ApplyDescription()
    {
        descriptionImage.sprite = itemSprite;

        descriptionText.text = itemDescription;

        descriptionTitle.text = itemName;
    }

    private void Update()
    {
        //debug only, to check if erasing the item worked
        if (Input.GetKeyDown(KeyCode.V))
        {
            EraseItem();
        }
    }

    //event that plays when you press the item slot
    public void OnPointerClick(PointerEventData eventData)
    {
        //checks if it's full first
        if (isFull)
        {
            //checks if it's a right or left click
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                //if it's item code is 3 (a damaging item) it will return early and not take the quantity off as damaging items cant be used outside of combat
                if (!GameManager.Instance.playerTurn && itemCode == 3)
                {
                    GameManager.Instance.pauseMenu.SetActive(false);
                    GameManager.Instance.UpgradesNotAvailable();
                    GameManager.Instance.UsingItem(itemCode, itemName, effectQuantity);
                    GameManager.Instance.DisablingHand();
                    return;
                }

                //taking the quantity off and checking if it's zero, if it is earse the item
                quantity--;
                ChangeQuantity();
                GameManager.Instance.pauseMenu.SetActive(false);
                GameManager.Instance.UpgradesNotAvailable();
                GameManager.Instance.UsingItem(itemCode, itemName, effectQuantity);
                GameManager.Instance.DisablingHand();
                if (quantity == 0)
                {
                    EraseItem();
                }
            }
            //if the player right clicks, open the description box
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                GameManager.Instance.DisablingHand();

                descriptionBox.SetActive(true);

                ApplyDescription();
            }
        }
    }

    //erasing the item, its not full, no sprite, no quantity text and no item name
    public void EraseItem()
    {
        isFull = false;
        itemImage.sprite = null;
        quantityText.enabled = false;
        itemName = "";
    }

    //checking if the pointer entered the item slot area, putting the hand there
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isFull)
        {
            GameObject hand = GameObject.Find("Hand 1");

            hand.transform.position = position.position;
        }
    }

    //checking if the pointer exited the item slot area, taking the hand off "disabling it"
    public void OnPointerExit(PointerEventData eventData)
    {
        if (isFull)
        {
            GameManager.Instance.DisablingHand();
        }
    }
}
