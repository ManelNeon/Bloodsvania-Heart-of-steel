using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopSlots : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    // ----- Item Data ----- //

    //the items name
    string itemName;

    //the items quantity (important to send to how many items the player wants to buy)
    int quantity;

    //the items sprite
    Sprite itemSprite;

    //the items description
    string itemDescription;

    //the items cost in gold
    int itemCost;

    //the items original cost
    int originalItemCost;

    //the items ID
    int itemID;

    //the player's stats
    Player playerStats;

    //the inventory manager
    InventoryManager inventoryManager;
    
    //the shop manager
    ShopManager shopManager;

    //an index for loops
    int index;

    //a bool isPlaying for text that plays
    bool isPlaying;

    //a is full bool to check if the slot is full
    [HideInInspector] public bool isFull;

    // ---- Shop Slot ---- //

    //the items image 
    [SerializeField] Image itemImage;

    //the position in which the hand will be
    [SerializeField] Transform position;

    //the button manager
    [SerializeField] ButtonManager buttonManager;

    //the warning display
    [SerializeField] GameObject warningDisplay;

    //and the warnings text
    [SerializeField] TextMeshProUGUI warningText;

    // ---- Item Description, in the SHOP UI ---- //

    //the items description box
    [SerializeField] GameObject descriptionBox;

    //the items Image in the description box
    [SerializeField] Image descriptionImage;

    //the description text object
    [SerializeField] TextMeshProUGUI descriptionText;

    //the items name object
    [SerializeField] TextMeshProUGUI descriptionTitle;

    //the items cost text
    [SerializeField] TextMeshProUGUI itemCostText;

    //the items quantity text
    [SerializeField] TextMeshProUGUI quantityText;

    //the buy button
    [SerializeField] Button buyButton;

    //the adding quantity button
    [SerializeField] Button addQuantityButton;

    //the removing quantity button
    [SerializeField] Button removeQuantityButton;

    //when we disable the slot, it is now not full and the items sprite is null
    private void OnDisable()
    {
        isFull = false;

        itemImage.sprite = null;
    }

    //adding the item to the slot and getting the shop manager
    public void AddItem(string itemName, Sprite itemSprite, string itemDescription, int itemCost, int itemCode, int effectQuantity, string itemEffect, int index, int itemID ,ShopManager shopManager)
    {
        playerStats = GameObject.Find("PlayerStatsHolder").GetComponent<Player>();

        inventoryManager = GameObject.Find("PlayerStatsHolder").GetComponent<InventoryManager>();

        this.itemName = itemName;

        this.itemSprite = itemSprite;

        this.itemDescription = itemDescription;

        this.originalItemCost = itemCost;

        this.itemCost = originalItemCost;

        this.index = index;

        this.itemID = itemID;

        this.shopManager = shopManager;

        isFull = true;

        itemImage.enabled = true;
        itemImage.sprite = itemSprite;
    }

    //applying the description
    void ApplyDescription()
    {
        descriptionImage.sprite = itemSprite;

        descriptionText.text = itemDescription;

        descriptionTitle.text = itemName;

        itemCostText.text = itemCost.ToString();
    }

    //when we click it the description will appear
    public void OnPointerClick(PointerEventData eventData)
    {
        if (isFull)
        {
            //the quantity is 1
            quantity = 1;

            //the items cost is the original item cost
            itemCost = originalItemCost;

            //the quantity text is set to the quantity
            quantityText.text = quantity.ToString();

            //the description box is activated
            descriptionBox.SetActive(true);

            //we remove every event in tthe buttons
            buyButton.onClick.RemoveAllListeners();

            //on the buy button we add the function buy item
            buyButton.onClick.AddListener(BuyItem);

            addQuantityButton.onClick.RemoveAllListeners();

            //on the add quantity button we add the function add item quantity
            addQuantityButton.onClick.AddListener(AddItemQuantity);

            removeQuantityButton.onClick.RemoveAllListeners();

            //on the remove quantity button we add the function remove item quantity
            removeQuantityButton.onClick.AddListener(RemoveItemQuantity);

            //we apply the description
            ApplyDescription();
        }
    }

    //function to buy the item
    void BuyItem()
    {
        //if the player has enough golld
        if (playerStats.gold >= itemCost)
        {
            //we add the item
            inventoryManager.AddItem(itemID, quantity);

            //we take the players gold
            playerStats.gold -= itemCost;

            //and we deactivate the shop
            buttonManager.DeactivateShop();
            
            //if any text is playing on the warning display we stop all coroutines and play our text
            if (isPlaying)
            {
                StopAllCoroutines();
            }

            GameManager.Instance.StartCoroutine(ItemBoughtTextPlay());
        }
        //if the player doesnt have enough currency
        else
        {
            if (isPlaying)
            {
                StopAllCoroutines();
            }

            //we deactivate the shop
            buttonManager.DeactivateShop();

            //and we warn the player he has no gold to play
            GameManager.Instance.StartCoroutine(NotEnoughGoldTextPlay());
        }
    }

    //function from when the player buys the item
    IEnumerator ItemBoughtTextPlay()
    {
        if (!isPlaying)
        {
            isPlaying = true;

            string text = "You bought " + quantity + " " + itemName + " for " + itemCost + " Gold!!!";

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
        }
    }

    //function for when he doenst have enough gold
    IEnumerator NotEnoughGoldTextPlay()
    {
        if (!isPlaying)
        {
            isPlaying = true;

            string text = "You don't have enough gold pounds...";

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
        }
    }

    //adding quantity
    void AddItemQuantity()
    {
        //a maximum of 999
        if (quantity < 999)
        {
            //we add the quantity, set add to its cost the original item cost and we set the texts to the corresponding values
            quantity++;
            itemCost += originalItemCost;
            itemCostText.text = itemCost.ToString();
            quantityText.text = quantity.ToString();
        }
    }

    //removing quantity
    void RemoveItemQuantity()
    {
        //no less than 1
        if (quantity > 1)
        {
            //we remove the quantity, in the item cost we take the original item cost and we set the texts to the corresponding values
            quantity--;
            itemCost -= originalItemCost;
            itemCostText.text = itemCost.ToString();
            quantityText.text = quantity.ToString();
        }
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
