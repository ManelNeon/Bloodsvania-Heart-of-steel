using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//child of the all mighty NPC class for when we want to give player a special
public class NPCSpecialGiver : NPC
{
    //getting the specialPrefab
    [SerializeField] Special special;

    //getting the special manager
    SpecialManager specialManager;

    //getting the warning text display
    [SerializeField] GameObject warningTextDisplay;

    //and getting its text
    [SerializeField] TextMeshProUGUI warningText;

    //bool for if we are playing the text
    bool playingText;

    public override void Start()
    {
        //getting the special manager
        specialManager = GameObject.Find("PlayerStatsHolder").GetComponent<SpecialManager>();

        base.Start();
    }

    //overriden function of the next line
    public override void NextLine()
    {
        //same here
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

            //in here we check if the player has the special, in case he doenst, we learn it
            if (!specialManager.CheckSpecial(special.specialName))
            {
                special.LearnSpecial();

                //if the text is playing we stop all coroutines
                if (playingText)
                {
                    StopAllCoroutines();
                }

                // we start the type text function
                StartCoroutine(SpecialLearnedTextPlay());
            }

            //we lock the mouse
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            player.GetComponent<PlayerController>().enabled = true;
        }
    }

    //telling the player which special he learned
    IEnumerator SpecialLearnedTextPlay()
    {
        playingText = true;

        warningText.text = "";

        warningTextDisplay.SetActive(true);

        string text = "You just learned " + special.specialName + " !!!";

        foreach (char c in text)
        {
            warningText.text += c;
            yield return new WaitForSeconds(GameManager.Instance.textSpeed);
        }

        playingText = false;

        yield return new WaitForSeconds(2.5f);

        warningTextDisplay.SetActive(false);

        yield break;
    }
}
