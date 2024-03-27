using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    //array with all the item's slots
    public ItemSlot[] itemSlot;

    [SerializeField] Item[] itemsPrefabs;

    int index;

    //adding a item, similar and identical to the special code
    public void AddItem(int itemID, int quantity)
    {
        for (int i = 0; i < itemsPrefabs.Length; i++)
        {
            if (itemsPrefabs[i].itemID == itemID)
            {
                index = i;
            }
        }

        for (int i = 0; i < itemSlot.Length; i++)
        {
            Item currentItem = itemsPrefabs[index];
            //in here we see if the item's name is equal, if it is we only need to add to the quantity
            if (itemSlot[i].itemID == currentItem.itemID)
            {
                itemSlot[i].quantity += quantity;
                itemSlot[i].ChangeQuantity();
                return;
            }
            if (!itemSlot[i].isFull)
            {
                if (quantity == 0)
                {
                    quantity = 1;
                }
                itemSlot[i].AddItem(currentItem.itemName, quantity, currentItem.itemSprite, currentItem.itemDescription, currentItem.itemCode, currentItem.effectQuantity, currentItem.itemEffect, itemID);
                return;
            }
        }
        Debug.Log("Inventory Full");
    }

    public bool LookForItem(string itemName)
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            if (itemSlot[i].itemName == itemName)
            {
                itemSlot[i].EraseItem();
                return true;
            }
        }
        return false;
    }

    public ItemSlot GetItemSlot(int i)
    {
        if (itemSlot[i].isFull)
        {
            return itemSlot[i];
        }
        return null;
    }
}
