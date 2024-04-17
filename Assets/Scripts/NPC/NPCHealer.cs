using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCHealer : NPC
{
    [SerializeField] Player playerStats;

    public override void NextLine()
    {
        if (index < dialogues.Length - 1)
        {
            index++;
            displayText.text = "";
            ChangingSprite();
            StartCoroutine(TypeLine());
        }
        //if we have reached the end of the array, we set the npc box to false, deactivate the cursor, it is no longer playing and the player controller is enabled
        else
        {
            npcTextBox.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            isPlaying = false;

            playerStats.currentHealth = playerStats.maxHealth;

            playerStats.currentMana = playerStats.maxMana;

            player.GetComponent<PlayerController>().enabled = true;
        }
    }
}
