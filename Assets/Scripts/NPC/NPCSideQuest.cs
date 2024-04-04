using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//really no use for it to be the child of NPC except to facilitate when we interact with it, almost every variable is repeated and every function is rewritten
public class NPCSideQuest : NPC
{
    [Header("Quest Related")]

    //the quest's description
    [SerializeField][TextArea] string questDescription;

    //the quest's name
    [SerializeField] string questName;

    //the quest giver name
    [SerializeField] string questGiver;

    //the quest's id
    public int questID;

    //the quest's reward
    [SerializeField] int questReward;

    //the quest's item name
    [SerializeField] string questItemName;

    //the quest manager
    [SerializeField] QuestManager questManager;

    [Header("Dialogue Related")]

    //the dialogues that appear when we accept the quest
    [SerializeField][TextArea] string[] dialoguesAcceptQuest;

    //the dialogues that appear when we refuse the quest
    [SerializeField][TextArea] string[] dialoguesRefuseQuest;

    //the dialogues that appear when we accept the quest and we are doing it
    [SerializeField][TextArea] string[] dialogueWhileQuestAccepted;

    //the dialogues that appear when we complete the quest
    [SerializeField][TextArea] string[] dialoguesCompletedQuest;

    //the buttons object
    [SerializeField] GameObject buttons;

    //the accept button
    [SerializeField] Button acceptButton;

    //the refuse button
    [SerializeField] Button refuseButton;

    //a bool to check if we accepted the quest
    bool questAcceptedFirst;

    //a bool to check if we refused the quest
    bool questRefused;

    //a bool to see if we accepted the quest and play the dialogue while we do the quest
    [HideInInspector] public bool questAcceptedSecond;

    //a bool to see if the quest is done
    [HideInInspector] public bool questCompleted;

    //a bool to check if the reward was given
    bool rewardGiven;

    //the overriden update
    public override void Update()
    {
        //we get the left button click
        if (Input.GetMouseButtonDown(0))
        {
            //if its playing
            if (isPlaying)
            {
                //we check if the quest isnt accepted or refused
                if (!questAcceptedFirst && !questAcceptedSecond && !questRefused)
                {
                    // we play the normal dialogue
                    if (displayText.text == dialogues[index])
                    {
                        if (index != dialogues.Length - 1)
                        {
                            NextLine();
                        }
                    }
                    else
                    {
                        audioSource.Stop();
                        StopAllCoroutines();
                        displayText.text = dialogues[index];
                    }
                }
                //we check if the quest was refused
                else if (questRefused)
                {
                    //we get the dialogue for when we refse the quest
                    if (displayText.text == dialoguesRefuseQuest[index])
                    {
                        NextLine();
                    }
                    else
                    {
                        audioSource.Stop();
                        StopAllCoroutines();
                        displayText.text = dialoguesRefuseQuest[index];
                    }
                }
                //we check if the quest is completed and play the corresponding dialogue
                else if (questCompleted)
                {
                    if (displayText.text == dialoguesCompletedQuest[index])
                    {
                        NextLine();
                    }
                    else
                    {
                        audioSource.Stop();
                        StopAllCoroutines();
                        displayText.text = dialoguesCompletedQuest[index];
                    }
                }
                //we check if the quest was accepted and we play the corresponding dialogue
                else if (questAcceptedFirst)
                {
                    if (displayText.text == dialoguesAcceptQuest[index])
                    {
                        NextLine();
                    }
                    else
                    {
                        audioSource.Stop();
                        StopAllCoroutines();
                        displayText.text = dialoguesAcceptQuest[index];
                    }
                }
                //we check if the quest accepted second is true and we play the dialogue for when we are doing the quest
                else if (questAcceptedSecond)
                {
                    if (displayText.text == dialogueWhileQuestAccepted[index])
                    {
                        NextLine();
                    }
                    else
                    {
                        audioSource.Stop();
                        StopAllCoroutines();
                        displayText.text = dialogueWhileQuestAccepted[index];
                    }
                }
            }
        }
    }

    //we override the start dialogue
    public override void StartDialogue()
    {

        //we get the inventory manager
        InventoryManager inventoryManager = GameObject.Find("PlayerStatsHolder").GetComponent<InventoryManager>();


        //if we are doing the quest we look for the item and if it exists we complete the quest (in the inventory manager we delete the item)
        if (questAcceptedSecond)
        {
            if (inventoryManager.LookForItem(questItemName))
            {
                questCompleted = true;
            }
        }

        //when we start the dialogue the quest refused is false so that the player may take the quest whenever he wants
        questRefused = false;

        //we put the hema starting to false ALWAYS
        isHemaStarting = false;

        base.StartDialogue();

    }


    //similar to the case in the update function, we check the bools and play the corresponding dialogue
    public override IEnumerator TypeLine()
    {
        if (!questAcceptedFirst && !questAcceptedSecond && !questRefused)
        {
            foreach (char c in dialogues[index].ToCharArray())
            {
                displayText.text += c;
                audioSource.Stop();
                audioSource.PlayOneShot(textTypingSound);
                yield return new WaitForSeconds(GameManager.Instance.textSpeed);
            }
            if (buttons.activeInHierarchy)
            {
                index = 0;
            }
        }
        else if (questRefused)
        {
            foreach (char c in dialoguesRefuseQuest[index].ToCharArray())
            {
                displayText.text += c;
                audioSource.Stop();
                audioSource.PlayOneShot(textTypingSound);
                yield return new WaitForSeconds(GameManager.Instance.textSpeed);
            }
        }
        else if (questCompleted)
        {
            CompleteQuest();

            foreach (char c in dialoguesCompletedQuest[index].ToCharArray())
            {
                displayText.text += c;
                audioSource.Stop();
                audioSource.PlayOneShot(textTypingSound);
                yield return new WaitForSeconds(GameManager.Instance.textSpeed);
            }
        }
        else if (questAcceptedFirst)
        {
            foreach (char c in dialoguesAcceptQuest[index].ToCharArray())
            {
                displayText.text += c;
                audioSource.Stop();
                audioSource.PlayOneShot(textTypingSound);
                yield return new WaitForSeconds(GameManager.Instance.textSpeed);
            }
        }
        else if (questAcceptedSecond)
        {
            foreach (char c in dialogueWhileQuestAccepted[index].ToCharArray())
            {
                displayText.text += c;
                audioSource.Stop();
                audioSource.PlayOneShot(textTypingSound);
                yield return new WaitForSeconds(GameManager.Instance.textSpeed);
            }
        }

        audioSource.Stop();

        yield break;
    }

    //similar to previous cases
    public override void NextLine()
    {
        if (!questAcceptedFirst && !questAcceptedSecond && !questRefused)
        {
            if (index < dialogues.Length - 1)
            {
                index++;
                displayText.text = "";
                if (index == dialogues.Length - 1)
                {
                    //when we react the end we set the buttons to active and we add the events to it
                    buttons.SetActive(true);
                    acceptButton.onClick.AddListener(AcceptQuest);
                    refuseButton.onClick.AddListener(RefuseQuest);
                }
                ChangingSprite();
                StartCoroutine(TypeLine());
            }
        }
        else if (questRefused)
        {
            if (index < dialoguesRefuseQuest.Length - 1)
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

                //in here we lock the mouse
                LockMouse();
                player.GetComponent<PlayerController>().enabled = true;
            }
        }
        else if(questCompleted)
        {
            if (index < dialoguesCompletedQuest.Length - 1)
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

                //in here too
                LockMouse();
                player.GetComponent<PlayerController>().enabled = true;
            }
        }
        else if(questAcceptedFirst)
        {
            if (index < dialoguesAcceptQuest.Length - 1)
            {
                index++;
                displayText.text = "";
                ChangingSprite();
                StartCoroutine(TypeLine());
            }
            else
            {
                npcTextBox.SetActive(false);

                //in here we put the quest accepted first in false and then we set the quest accepted second to true
                questAcceptedFirst = false;

                questAcceptedSecond = true;

                isPlaying = false;

                LockMouse();
                player.GetComponent<PlayerController>().enabled = true;
            }
        }
        else if(questAcceptedSecond)
        {
            if (index < dialogueWhileQuestAccepted.Length - 1)
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

                LockMouse();
            }
        }

    }

    //function that locks the mouse
    void LockMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    //function that gives the player the reward
    void CompleteQuest()
    {
        if (!rewardGiven)
        {
            rewardGiven = true;

            questManager.CompleteQuest(questReward, questName);
        }
    }


    //the function we give the buttons, we remove all events on it, we set the index to zero again, we put the display text to zero, we type the line and we add the quest
    void AcceptQuest()
    {
        questAcceptedFirst = true;
        acceptButton.onClick.RemoveAllListeners();
        index = 0;
        displayText.text = "";
        StartCoroutine(TypeLine());
        GameManager.Instance.DisablingHand();
        questManager.AddQuest(questName,questGiver,questDescription, questID);
        buttons.SetActive(false);
    }

    void RefuseQuest()
    {
        questRefused = true;
        refuseButton.onClick.RemoveAllListeners();
        index = 0;
        displayText.text = "";
        StartCoroutine(TypeLine());
        GameManager.Instance.DisablingHand();
        buttons.SetActive(false);
    }
}
