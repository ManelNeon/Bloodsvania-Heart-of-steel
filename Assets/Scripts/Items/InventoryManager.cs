using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    //array with all the item's slots
    public ItemSlot[] itemSlot;

    //the item prefabs
    [SerializeField] Item[] itemsPrefabs;

    //a index checking the item prefabs
    int index;

    //adding a item, similar and identical to the special code
    public void AddItem(int itemID, int quantity)
    {
        for (int i = 0; i < itemsPrefabs.Length; i++)
        {
            //we check in the item prefabs for the same itemID and then store the index if it matches
            if (itemsPrefabs[i].itemID == itemID)
            {
                index = i;
            }
        }

        //in here we get the current item and then store the item
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
            //if the item isnt full we add the item to the slot
            if (!itemSlot[i].isFull)
            {
                //if quantity is zero (likely if the item is picked off the ground sometimes), the quantity will be one
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

    //we return a true or false if the item exists (necessary for the side quests)
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

    //we get the item slot corresponding to the index in the data manager, by checking if it's full
    public ItemSlot GetItemSlot(int i)
    {
        if (itemSlot[i].isFull)
        {
            return itemSlot[i];
        }
        return null;
    }
}
