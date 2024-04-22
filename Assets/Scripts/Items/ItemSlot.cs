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

    [HideInInspector] public int itemID;

    Sprite itemSprite;

    string itemDescription;

    /*
    0 - Key Item
    1 - Healing
    2 - Regening Blood
    3 - Debuffing the enemy
    */
    [HideInInspector] public int itemCode;

    int effectQuantity;

    string itemEffect;

    [HideInInspector] public bool isFull;

    bool isPlaying;

    // ---- Item Slot ---- //

    [SerializeField] TMP_Text quantityText;

    [SerializeField] Image itemImage;

    [SerializeField] Transform position;

    [SerializeField] ButtonManager buttonManager;

    [SerializeField] GameObject warningDisplay;

    [SerializeField] TextMeshProUGUI warningText;

    // ---- Item Description ---- //

    [SerializeField] GameObject descriptionBox;

    [SerializeField] Image descriptionImage;

    [SerializeField] TextMeshProUGUI descriptionText;

    [SerializeField] TextMeshProUGUI descriptionTitle;

    [SerializeField] TextMeshProUGUI descriptionEffect;

    //adding the item to the slot
    public void AddItem(string itemName, int quantity, Sprite itemSprite, string itemDescription, int itemCode, int effectQuantity, string itemEffect, int itemID)
    {
        this.itemName = itemName;

        this.quantity = quantity;

        this.itemSprite = itemSprite;

        this.itemDescription = itemDescription;

        this.itemCode = itemCode;

        this.effectQuantity = effectQuantity;

        this.itemEffect = itemEffect;

        this.itemID = itemID;

        isFull = true;

        ChangeQuantity();
        quantityText.enabled = true;

        itemImage.enabled = true;
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

        descriptionEffect.text = itemEffect;
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
                    GameManager.Instance.DisablingHand();
                    if (isPlaying)
                    {
                        StopAllCoroutines();
                        isPlaying = false;
                    }
                    buttonManager.StartCoroutine(WarningDisplayEvent());
                    buttonManager.ResumeGame();
                    return;
                }

                //if the tiems code is 0 (a key item) it will return early, not take the quantity off and display a warning
                if (itemCode == 0)
                {
                    GameManager.Instance.UsingItem(itemCode, itemName, effectQuantity);
                    GameManager.Instance.DisablingHand();
                    if (isPlaying)
                    {
                        StopAllCoroutines();
                        isPlaying = false;
                    }
                    buttonManager.StartCoroutine(WarningDisplayKeyItem());
                    buttonManager.ResumeGame();
                    return;
                }

                //taking the quantity off and checking if it's zero, if it is earse the item
                quantity--;
                ChangeQuantity();
                GameManager.Instance.UsingItem(itemCode, itemName, effectQuantity);
                GameManager.Instance.DisablingHand();
                if (quantity == 0)
                {
                    EraseItem();
                }
                buttonManager.ResumeGame();
            }
            //if the player right clicks, open the description box
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                descriptionBox.SetActive(true);

                ApplyDescription();
            }
        }
    }

    //display when we try to use a key item outside of combat
    IEnumerator WarningDisplayKeyItem()
    {
        isPlaying = true;

        string text = "You can't use key items...";

        if (GameManager.Instance.playerTurn)
        {
            warningDisplay.SetActive(true);

            warningText.text = "";

            foreach (char c in text)
            {
                warningText.text += c;
                yield return new WaitForSeconds(GameManager.Instance.textSpeed);
            }

            yield return new WaitForSeconds(3);

            warningDisplay.SetActive(false);

            isPlaying = false;

            yield break;
        }
        else
        {
            warningDisplay.SetActive(true);

            warningText.text = "";

            foreach (char c in text)
            {
                warningText.text += c;
                yield return new WaitForSeconds(GameManager.Instance.textSpeed);
            }

            yield return new WaitForSeconds(3);

            warningDisplay.SetActive(false);

            isPlaying = false;

            yield break;
        }
    }

    //display when we try to use a damaging item outside of combat
    IEnumerator WarningDisplayEvent()
    {
        isPlaying = true;

        warningDisplay.SetActive(true);

        string text = "You can't use damaging items outside of combat...";

        warningText.text = "";

        foreach (char c in text)
        {
            warningText.text += c;
            yield return new WaitForSeconds(GameManager.Instance.textSpeed);
        }

        yield return new WaitForSeconds(3);

        warningDisplay.SetActive(false);

        isPlaying = false;

        yield break;
    }

    //erasing the item, its not full, no sprite, no quantity text and no item name
    public void EraseItem()
    {
        isFull = false;
        itemImage.enabled = false;
        itemImage.sprite = null;
        quantityText.enabled = false;
        itemID = 999;
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
