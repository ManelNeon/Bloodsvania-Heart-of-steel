using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//code that contains the item variables
public class Item : MonoBehaviour
{
    //the item's name
    [SerializeField] string itemName;

    //the quantity of the item
    [SerializeField] int quantity;

    //the sprite of the item
    [SerializeField] Sprite itemSprite;

    [Header("Good/Bad/Unknown")][SerializeField] string itemEffect;

    //the quantity that the effect will do (how much healing, how much damage etc...)
    [SerializeField] int effectQuantity;

    /*
    0 - Key Item
    1 - Healing
    2 - Regening Blood
    3 - Debuffing the enemy
    */
    [SerializeField] int itemCode;

    //the description of the item
    [TextArea][SerializeField] string itemDescription;

    private void Update()
    {
        //debug only, to see if the items worked
        if (Input.GetKeyDown(KeyCode.X))
        {
            GetItem();
        }
    }

    //code to get the item
    void GetItem()
    {
        GameObject.Find("PlayerStatsHolder").GetComponent<InventoryManager>().AddItem(itemName, quantity, itemSprite, itemDescription, itemCode, effectQuantity, itemEffect);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GetItem();
            Destroy(gameObject);
        }
    }
}
