using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//child of all mighty fader NPC
public class NPCMerchant : NPC
{
    //in here we only get the shop manager, which every Merchant NPC has one
    ShopManager shopManager;

    private void Start()
    {
        //we get it
        shopManager = GetComponent<ShopManager>();
    }

    //we override the next line funtion
    public override void NextLine()
    {
        //the beggining is the same
        if (index < dialogues.Length - 1)
        {
            index++;
            displayText.text = "";
            ChangingSprite();
            StartCoroutine(TypeLine());
        }
        //yet when it ends we set the shop's manager index to zero and we get the items
        else
        {
            npcTextBox.SetActive(false);

            isPlaying = false;

            shopManager.index = 0;

            shopManager.GettingItems();
        }
    }
}
