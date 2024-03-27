using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//code for the special Slots on the UI, has a clicker, enter and exit event
public class QuestSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    // -------- QUEST DATA -------- //

    //the quest's name for the UI
    [HideInInspector] public string questName;

    //the typing Code, for the UI
    string questGiver;

    string questDescription;

    [HideInInspector] public int questID;

    //checking if the slot is full
    [HideInInspector] public bool isFull;

    // -------- QUEST SLOT -------- //

    //text for the quest's name
    [SerializeField] TextMeshProUGUI questNameText;

    //position to where the pointer will go
    [SerializeField] Transform position;

    [SerializeField] GameObject divider;

    // ------- QUEST DESCRIPTION ------ //

    //object that contains all the descriptive features of the quest
    [SerializeField] GameObject questDescriptionBox;

    //object that will contain the name of quest giver
    [SerializeField] TextMeshProUGUI questGiverText;

    //object that will contain the description of the quest
    [SerializeField] TextMeshProUGUI questDescriptionText;

    //code that adds a special, requires the name of the special, it's typing Code, the time it takes to use the special, its image and the description
    public void AddQuest(string questName, string questGiver, string questDescription, int questID)
    {
        this.questName = questName;

        this.questGiver = questGiver;

        this.questDescription = questDescription;

        this.questID = questID;

        divider.SetActive(true);

        //it is now a full slot
        isFull = true;

        //assigning the corresponding typing text according to the typing text

        //changing the text to the special's name and it's typing
        questNameText.text = questName;

        //enabling the text
        questNameText.enabled = true;
    }

    public void CompleteQuest(int questReward)
    {
        GameObject.Find("PlayerStatsHolder").GetComponent<Player>().gold += questReward;

        isFull = false;

        questNameText.enabled = false;

        divider.SetActive(false);
    }

    //On click events, when it's the player turn and when its not (the player can only open up the menu while in combat if it's his turn)
    public void OnPointerClick(PointerEventData eventData)
    {
        ShowDescriptionBox();
    }

    //code to show the description box and assign the corresponding variables
    void ShowDescriptionBox()
    {
        questGiverText.text = questGiver;

        questDescriptionText.text = questDescription;

        questDescriptionBox.SetActive(true);
    }

    //putting the hand on the position I want it too, and setting its scale lower
    public void OnPointerEnter(PointerEventData eventData)
    {
        GameObject hand = GameObject.Find("Hand 1");

        hand.transform.position = position.position;
    }

    //getting the hand out of the way, puff and its scale back to normal
    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.Instance.DisablingHand();
    }


}
