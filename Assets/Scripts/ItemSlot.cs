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

    public void AddItem(string itemName, int quantity, Sprite itemSprite, string itemDescription, int itemCode)
    {
        this.itemName = itemName;

        this.quantity = quantity;

        this.itemSprite = itemSprite;

        this.itemDescription = itemDescription;

        this.itemCode = itemCode;

        isFull = true;

        quantityText.text = quantity.ToString();
        quantityText.enabled = true;

        itemImage.sprite = itemSprite;
    }

    void ApplyDescription()
    {
        descriptionImage.sprite = itemSprite;

        descriptionText.text = itemDescription;

        descriptionTitle.text = itemName;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isFull)
        {
            GameManager.Instance.DisablingHand();

            descriptionBox.SetActive(true);

            ApplyDescription();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isFull)
        {
            GameObject hand = GameObject.Find("Hand 1");

            hand.transform.position = position.position;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isFull)
        {
            GameManager.Instance.DisablingHand();
        }
    }
}
