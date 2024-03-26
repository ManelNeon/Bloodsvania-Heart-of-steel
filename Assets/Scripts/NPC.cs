using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    [TextArea] public string[] dialogues;

    public bool isHemaStarting;

    public Sprite hemaSprite;

    public Sprite npcSprite;

    public Image personTalkingImage;

    public GameObject npcTextBox;

    public TextMeshProUGUI displayText;

    public GameObject player;

    [HideInInspector] public int index;

    [HideInInspector]public bool isPlaying;

    public virtual void Update() 
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isPlaying)
            {
                if (displayText.text == dialogues[index])
                {
                    NextLine();
                }
                else
                {
                    StopAllCoroutines();
                    displayText.text = dialogues[index];
                }
            }
        }
    }

    public virtual void StartDialogue()
    {
        index = 0;

        isPlaying = true;

        npcTextBox.SetActive(true);

        ChangingSprite();

        displayText.text = "";

        StartCoroutine(TypeLine());
    }

    public void ChangingSprite()
    {
        if (isHemaStarting)
        {
            personTalkingImage.sprite = hemaSprite;
            isHemaStarting = false;
        }
        else
        {
            personTalkingImage.sprite = npcSprite;
            isHemaStarting = true;
        }
    }

    public virtual IEnumerator TypeLine()
    {
        foreach (char c in dialogues[index].ToCharArray())
        {
            displayText.text += c;
            yield return new WaitForSeconds(GameManager.Instance.textSpeed);
        }
        yield break;
    }

    
    public virtual void NextLine()
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

            player.GetComponent<PlayerController>().enabled = true;
        }
    }
}
