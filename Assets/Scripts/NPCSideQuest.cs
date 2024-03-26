using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCSideQuest : NPC
{
    [Header("Quest Related")]

    [SerializeField][TextArea] string questDescription;

    [SerializeField] string questName;

    [SerializeField] string questGiver;

    [SerializeField] int questReward;

    [SerializeField] string questItemName;

    [SerializeField] QuestManager questManager;

    [Header("Dialogue Related")]
    [SerializeField][TextArea] string[] dialoguesAcceptQuest;

    [SerializeField][TextArea] string[] dialoguesRefuseQuest;

    [SerializeField][TextArea] string[] dialogueWhileQuestAccepted;

    [SerializeField][TextArea] string[] dialoguesCompletedQuest;

    [SerializeField] GameObject buttons;

    [SerializeField] Button acceptButton;

    [SerializeField] Button refuseButton;

    bool questAcceptedFirst;

    bool questRefused;

    bool questAcceptedSecond;

    bool questCompleted;

    bool rewardGiven;

    // Update is called once per frame
    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            questCompleted = true;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (isPlaying)
            {
                if (!questAcceptedFirst && !questAcceptedSecond && !questRefused)
                {
                    if (displayText.text == dialogues[index])
                    {
                        if (index != dialogues.Length - 1)
                        {
                            NextLine();
                        }
                    }
                    else
                    {
                        StopAllCoroutines();
                        displayText.text = dialogues[index];
                    }
                }
                else if (questRefused)
                {
                    if (displayText.text == dialoguesRefuseQuest[index])
                    {
                        NextLine();
                    }
                    else
                    {
                        StopAllCoroutines();
                        displayText.text = dialoguesRefuseQuest[index];
                    }
                }
                else if (questCompleted)
                {
                    if (displayText.text == dialoguesCompletedQuest[index])
                    {
                        NextLine();
                    }
                    else
                    {
                        StopAllCoroutines();
                        displayText.text = dialoguesCompletedQuest[index];
                    }
                }
                else if (questAcceptedFirst)
                {
                    if (displayText.text == dialoguesAcceptQuest[index])
                    {
                        NextLine();
                    }
                    else
                    {
                        StopAllCoroutines();
                        displayText.text = dialoguesAcceptQuest[index];
                    }
                }
                else if (questAcceptedSecond)
                {
                    if (displayText.text == dialogueWhileQuestAccepted[index])
                    {
                        NextLine();
                    }
                    else
                    {
                        StopAllCoroutines();
                        displayText.text = dialogueWhileQuestAccepted[index];
                    }
                }
            }
        }
    }

    public override void StartDialogue()
    {
        InventoryManager inventoryManager = GameObject.Find("PlayerStatsHolder").GetComponent<InventoryManager>();


        if (inventoryManager.LookForItem(questItemName) && questAcceptedSecond)
        {
            questCompleted = true;
        }

        questRefused = false;

        isHemaStarting = false;

        base.StartDialogue();

    }

    public override IEnumerator TypeLine()
    {
        if (!questAcceptedFirst && !questAcceptedSecond && !questRefused)
        {
            foreach (char c in dialogues[index].ToCharArray())
            {
                displayText.text += c;
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
                yield return new WaitForSeconds(GameManager.Instance.textSpeed);
            }
        }
        else if (questCompleted)
        {
            CompleteQuest();

            foreach (char c in dialoguesCompletedQuest[index].ToCharArray())
            {
                displayText.text += c;
                yield return new WaitForSeconds(GameManager.Instance.textSpeed);
            }
        }
        else if (questAcceptedFirst)
        {
            foreach (char c in dialoguesAcceptQuest[index].ToCharArray())
            {
                displayText.text += c;
                yield return new WaitForSeconds(GameManager.Instance.textSpeed);
            }
        }
        else if (questAcceptedSecond)
        {
            foreach (char c in dialogueWhileQuestAccepted[index].ToCharArray())
            {
                displayText.text += c;
                yield return new WaitForSeconds(GameManager.Instance.textSpeed);
            }
        }

        yield break;
    }

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

                questAcceptedFirst = false;

                questAcceptedSecond = true;

                isPlaying = false;

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
            }
        }
    }

    void CompleteQuest()
    {
        if (!rewardGiven)
        {
            rewardGiven = true;

            questManager.CompleteQuest(questReward, questName);
        }
    }


    void AcceptQuest()
    {
        questAcceptedFirst = true;
        acceptButton.onClick.RemoveAllListeners();
        index = 0;
        displayText.text = "";
        StartCoroutine(TypeLine());
        GameManager.Instance.DisablingHand();
        questManager.AddQuest(questName,questGiver,questDescription);
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
