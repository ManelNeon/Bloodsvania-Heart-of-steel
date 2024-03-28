using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//the father of NPC'S
public class NPC : MonoBehaviour
{
    //the dialogue array
    [TextArea] public string[] dialogues;

    //a bool to check if it's hema starting the dialoge or the NPC
    public bool isHemaStarting;

    //hema's sprite
    public Sprite hemaSprite;

    //the npc's sprite
    public Sprite npcSprite;

    //the place in which we will put the person talking's image
    public Image personTalkingImage;

    //the npc text box
    public GameObject npcTextBox;

    //the text mesh where we will put our dialogue
    public TextMeshProUGUI displayText;

    //and finally we get the player
    public GameObject player;

    //a hidden index to run through dialogues
    [HideInInspector] public int index;

    //a bool to check if the dialogue is playing
    [HideInInspector]public bool isPlaying;

    public virtual void Update() 
    {
        //if the player clicks on the left mouse
        if (Input.GetMouseButtonDown(0))
        {
            //if it's playing
            if (isPlaying)
            {
                //if the dialogue text is the same as the one on the UI
                if (displayText.text == dialogues[index])
                {
                    //we got to the next line
                    NextLine();
                }
                //if it isnt
                else
                {
                    //we stop all coroutines (the writing of the text)
                    StopAllCoroutines();
                    //and set the dialogue to be the same (one more click on the screen and the next line will play
                    displayText.text = dialogues[index];
                }
            }
        }
    }

    // this is for when we start the dialogue
    public virtual void StartDialogue()
    {
        //we set the index to zero
        index = 0;

        //the dialogue is now playing
        isPlaying = true;

        //the npc text box is active
        npcTextBox.SetActive(true);

        //we change the sprite
        ChangingSprite();

        //we put the display text to nothing
        displayText.text = "";

        //we unlock the mouse
        GameManager.Instance.UnlockingMouse();

        //we disable the player's controller
        player.GetComponent<PlayerController>().enabled = false;

        //and we start the corotine for the writing of the text
        StartCoroutine(TypeLine());
    }

    //the function that changes the sprite
    public void ChangingSprite()
    {
        //we check if the bool is true or false
        if (isHemaStarting)
        {
            //if its true the sprite we show is emma and we set the bool to false
            personTalkingImage.sprite = hemaSprite;
            isHemaStarting = false;
        }
        else
        {
            //if it isnt we set the sprite to the npc one and we set the bool to true
            personTalkingImage.sprite = npcSprite;
            isHemaStarting = true;
        }
    }

    //we write the text
    public virtual IEnumerator TypeLine()
    {
        foreach (char c in dialogues[index].ToCharArray())
        {
            displayText.text += c;
            yield return new WaitForSeconds(GameManager.Instance.textSpeed);
        }
        yield break;
    }

    
    //we get the next line, if its still less than the dialogue length we change the sprite again, add 1 to the index and start the coroutine again
    public virtual void NextLine()
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

            player.GetComponent<PlayerController>().enabled = true;
        }
    }
}
