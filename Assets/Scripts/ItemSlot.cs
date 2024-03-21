using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    //code that functions exactly the same as the special code, with the difference that now it contains a image

    string itemName;

    int quantity;

    Sprite itemSprite;

    [HideInInspector] public bool isFull;

    [SerializeField] TMP_Text quantityText;

    [SerializeField] Image itemImage;

    public void AddItem(string itemName, int quantity, Sprite itemSprite)
    {
        this.itemName = itemName;

        this.quantity = quantity;

        this.itemSprite = itemSprite;

        isFull = true;

        quantityText.text = quantity.ToString();
        quantityText.enabled = true;

        itemImage.sprite = itemSprite;
           
    }
}
