using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] Item[] items;

    [SerializeField] ShopSlots[] shopSlots;

    [SerializeField] GameObject shopMenu;

    [SerializeField] TextMeshProUGUI playerGoldShopText;

    [HideInInspector] public int index;

    public void GettingItems()
    {
        if (index < items.Length)
        {
            ApplyItemOnSlot();

            return;
        }
        else
        {
            Player playerStats = GameObject.Find("PlayerStatsHolder").GetComponent<Player>();

            playerGoldShopText.text = "You have " + playerStats.gold;

            shopMenu.SetActive(true);

        }
    }

    void ApplyItemOnSlot()
    {
        for (int i = 0; i < shopSlots.Length; i++)
        {
            if (!shopSlots[i].isFull)
            {
                shopSlots[i].AddItem(items[index].itemName, items[index].itemSprite, items[index].itemDescription, items[index].itemCost, items[index].itemCode, items[index].effectQuantity, items[index].itemEffect, index, items[index].itemID, this);

                index++;

                GettingItems();

                return;
            }
            
        }
    }
}
