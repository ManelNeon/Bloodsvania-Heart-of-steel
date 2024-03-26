using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPCSpecialGiver : NPC
{
    Special special;

    SpecialManager specialManager;

    [SerializeField] GameObject warningTextDisplay;

    [SerializeField] TextMeshProUGUI warningText;

    bool playingText;

    private void Start()
    {
        special = GetComponent<Special>();

        specialManager = GameObject.Find("PlayerStatsHolder").GetComponent<SpecialManager>();
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

            if (!specialManager.CheckSpecial(special.specialName))
            {
                special.LearnSpecial();

                if (playingText)
                {
                    StopAllCoroutines();
                }

                StartCoroutine(SpecialLearnedTextPlay());
            }

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            player.GetComponent<PlayerController>().enabled = true;
        }
    }

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
