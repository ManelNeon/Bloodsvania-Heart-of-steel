using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMerchant : NPC
{
    ShopManager shopManager;

    private void Start()
    {
        shopManager = GetComponent<ShopManager>();
    }

    public override void NextLine()
    {
        if (index < dialogues.Length - 1)
        {
            index++;
            displayText.text = "";
            ChangingSprite();
            StartCoroutine(TypeLine());
        }
        else
        {
            npcTextBox.SetActive(false);

            isPlaying = false;

            shopManager.index = 0;

            shopManager.GettingItems();
        }
    }
}
