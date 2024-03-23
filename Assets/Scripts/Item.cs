using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//code that contains the item variables
public class Item : MonoBehaviour
{
    [SerializeField] string itemName;

    [SerializeField] int quantity;

    [SerializeField] Sprite itemSprite;

    [TextArea][SerializeField] string itemDescription;

    /*
    1 - Healing
    2 - Regening Blood
    3 - Debuffing the enemy
    */
    [SerializeField] int itemCode;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            GetItem();
        }
    }

    void GetItem()
    {
        GameObject.Find("PlayerStatsHolder").GetComponent<InventoryManager>().AddItem(itemName, quantity, itemSprite, itemDescription, itemCode);
    }
}
