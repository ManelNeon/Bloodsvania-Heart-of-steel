using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    //array with all the item's slots
    public ItemSlot[] itemSlot;


    //adding a item, similar and identical to the special code
    public void AddItem(string itemName, int quantity, Sprite itemSprite, string itemDescription, int itemCode, int effectQuantity)
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            //in here we see if the item's name is equal, if it is we only need to add to the quantity
            if (itemSlot[i].itemName == itemName)
            {
                itemSlot[i].quantity += quantity;
                itemSlot[i].ChangeQuantity();
                return;
            }
            if (!itemSlot[i].isFull)
            {
                itemSlot[i].AddItem(itemName, quantity, itemSprite, itemDescription, itemCode, effectQuantity);
                return;
            }
        }
        Debug.Log("Inventory Full");
    }
}
