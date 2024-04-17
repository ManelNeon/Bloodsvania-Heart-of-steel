using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


//code that contains the item variables
public class Item : MonoBehaviour
{
    //the item's name
    public string itemName;

    //the quantity of the item
    public int quantity;

    //the sprite of the item
    public Sprite itemSprite;

    [Header("Good/Bad/Unknown")] public string itemEffect;

    //the quantity that the effect will do (how much healing, how much damage etc...)
    public int effectQuantity;

    public int itemCost;

    public int itemID;

    /*
    0 - Key Item
    1 - Healing
    2 - Regening Blood
    3 - Debuffing the enemy
    */
    public int itemCode;

    //the description of the item
    [TextArea] public string itemDescription;

    [SerializeField] GameObject warningDisplay;

    [SerializeField] TextMeshProUGUI warningDisplayText;

    //code to get the item
    void GetItem()
    {
        GameObject.Find("PlayerStatsHolder").GetComponent<InventoryManager>().AddItem(itemID,quantity);
    }

    private void OnEnable()
    {
        if (itemCode == 0)
        {
            QuestManager questManager = GameObject.Find("PlayerStatsHolder").GetComponent<QuestManager>();

            if (questManager.CheckingIfQuestComplete(gameObject.name))
            {
                gameObject.SetActive(false);
            }
        }
    }


    //for items in the wild, we get them by passing through them
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GetItem();
            Destroy(gameObject);
        }
    }

}
