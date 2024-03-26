using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopSlots : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    // ----- Item Data ----- //

    string itemName;

    int quantity;

    Sprite itemSprite;

    string itemDescription;

    int itemCost;

    int originalItemCost;

    /*
    0 - Key Item
    1 - Healing
    2 - Regening Blood
    3 - Debuffing the enemy
    */
    int itemCode;

    int effectQuantity;

    string itemEffect;

    Player playerStats;

    InventoryManager inventoryManager;
    
    ShopManager shopManager;

    int index;

    bool isPlaying;

    [HideInInspector] public bool isFull;

    // ---- Shop Slot ---- //

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

    [SerializeField] TextMeshProUGUI itemCostText;

    [SerializeField] TextMeshProUGUI quantityText;

    [SerializeField] Button buyButton;

    [SerializeField] Button addQuantityButton;

    [SerializeField] Button removeQuantityButton;

    private void OnDisable()
    {
        isFull = false;

        itemImage.sprite = null;
    }

    public void AddItem(string itemName, Sprite itemSprite, string itemDescription, int itemCost, int itemCode, int effectQuantity, string itemEffect, int index, ShopManager shopManager)
    {
        playerStats = GameObject.Find("PlayerStatsHolder").GetComponent<Player>();

        inventoryManager = GameObject.Find("PlayerStatsHolder").GetComponent<InventoryManager>();

        this.itemName = itemName;

        this.itemSprite = itemSprite;

        this.itemDescription = itemDescription;

        this.originalItemCost = itemCost;

        this.itemCost = originalItemCost;

        this.itemCode = itemCode;

        this.effectQuantity = effectQuantity;

        this.itemEffect = itemEffect;

        this.index = index;

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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isFull)
        {
            quantity = 1;

            itemCost = originalItemCost;

            quantityText.text = quantity.ToString();

            descriptionBox.SetActive(true);

            buyButton.onClick.RemoveAllListeners();

            buyButton.onClick.AddListener(BuyItem);

            addQuantityButton.onClick.RemoveAllListeners();

            addQuantityButton.onClick.AddListener(AddItemQuantity);

            removeQuantityButton.onClick.RemoveAllListeners();

            removeQuantityButton.onClick.AddListener(RemoveItemQuantity);

            ApplyDescription();
        }
    }

    void BuyItem()
    {
        if (playerStats.gold >= itemCost)
        {
            inventoryManager.AddItem(itemName, quantity, itemSprite, itemDescription, itemCode, effectQuantity, itemEffect);

            playerStats.gold -= itemCost;

            buttonManager.DeactivateShop();
            
            if (isPlaying)
            {
                StopAllCoroutines();
            }

            GameManager.Instance.StartCoroutine(ItemBoughtTextPlay());
        }
        else
        {
            if (isPlaying)
            {
                StopAllCoroutines();
            }

            buttonManager.DeactivateShop();

            GameManager.Instance.StartCoroutine(NotEnoughGoldTextPlay());
        }
    }

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

    void AddItemQuantity()
    {
        if (quantity < 999)
        {
            quantity++;
            itemCost += originalItemCost;
            itemCostText.text = itemCost.ToString();
            quantityText.text = quantity.ToString();
        }
    }

    void RemoveItemQuantity()
    {
        if (quantity > 1)
        {
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
