using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

//the shop manager
public class ShopManager : MonoBehaviour
{
    //the array containing the items the merchant has
    [SerializeField] Item[] items;

    //the shop slots in the ui
    [SerializeField] ShopSlots[] shopSlots;

    //the shop menu in the UI
    [SerializeField] GameObject shopMenu;

    //the text that says how much gold the player has
    [SerializeField] TextMeshProUGUI playerGoldShopText;

    //the index
    [HideInInspector] public int index;

    //getting the items
    public void GettingItems()
    {
        //if the index is lower than the items array length then we apply the item on the lost
        if (index < items.Length)
        {
            ApplyItemOnSlot();

            return;
        }
        //if not then we activate the shops menu and show the player's gold
        else
        {
            Player playerStats = GameObject.Find("PlayerStatsHolder").GetComponent<Player>();

            playerGoldShopText.text = "You have " + playerStats.gold;

            shopMenu.SetActive(true);

        }
    }

    //in here we apply the items on the slot, because in this game the items are infinite in the store, no need to send quantity
    void ApplyItemOnSlot()
    {
        //for loop that will check the shops length and check if its not full, if not a item will be added to that slot
        for (int i = 0; i < shopSlots.Length; i++)
        {
            if (!shopSlots[i].isFull)
            {
                shopSlots[i].AddItem(items[index].itemName, items[index].itemSprite, items[index].itemDescription, items[index].itemCost, items[index].itemCode, items[index].effectQuantity, items[index].itemEffect, index, items[index].itemID, this);

                //we then add the index
                index++;

                //and get the next item
                GettingItems();

                return;
            }
            
        }
    }
}
