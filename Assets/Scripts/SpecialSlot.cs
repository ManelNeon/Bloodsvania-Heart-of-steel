using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR;

//code for the special Slots on the UI, has a clicker, enter and exit event
public class SpecialSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    // -------- SPECIAL DATA -------- //

    //the special's name for the UI
    string specialName;

    //the typing Code, for the UI
    int typingCode;

    //time needed to tplay the special's animation
    float specialTime;

    //the typing for the UI
    string typing;

    //special's Description
    string specialDescription;

    //special's image
    Sprite specialImage;

    int specialCost;

    //checking if the slot is full
    [HideInInspector] public bool isFull;

    // -------- SPECIAL SLOT -------- //

    //text for the special
    [SerializeField] TMP_Text specialText;

    //position to where the pointer will go
    [SerializeField] Transform postion;

    // ------- SPECIAL DESCRIPTION ------ //

    //object that contains all the descriptive features of the special attack
    [SerializeField] GameObject descriptionBox;

    //object that will contain the image of the special attack
    [SerializeField] Image spriteSlot;

    //object that will contain the name of the special attack
    [SerializeField] TextMeshProUGUI specialNameText;

    //object that will contain the description of the special attack
    [SerializeField] TextMeshProUGUI specialDescriptionText;

    //code that adds a special, requires the name of the special, it's typing Code, the time it takes to use the special, its image and the description
    public void AddSpecial(string specialName, int typingCode, float specialTime, Sprite specialImage, string specialDescription, int specialCost)
    {
        //assigning the special's data to the spots
        this.specialName = specialName;

        this.typingCode = typingCode;

        this.specialTime = specialTime;

        this.specialImage = specialImage;

        this.specialDescription = specialDescription;

        this.specialCost = specialCost;

        //it is now a full slot
        isFull = true;

        //assigning the corresponding typing text according to the typing text
        AssignTyping(typingCode);

        //changing the text to the special's name and it's typing
        specialText.text = specialName + " T: " + typing;

        //enabling the text
        specialText.enabled = true;
    }

    //code that assigns the type according to the 5 types (Check enemy script to see what they are)
    void AssignTyping(int typingCode)
    {
        switch (typingCode)
        {
            case 1:

                typing = "Savage";

                break;
            case 2:

                typing = "Machine";

                break;
            case 3:

                typing = "Human";

                break;
            case 4:

                typing = "Fulgurite";

                break;
            case 5:

                typing = "Nature";

                break;
            default:

                typing = "";

                break;
        }
    }

    //On click events, when it's the player turn and when its not (the player can only open up the menu while in combat if it's his turn)
    public void OnPointerClick(PointerEventData eventData)
    {
        if (GameManager.Instance.playerTurn)
        {
            //if the player left clicks while fighting
            if(eventData.button == PointerEventData.InputButton.Left)
            {
                //the pause menu will deactivate, we will start a courotine with the special attack and we will disable the hand
                GameManager.Instance.pauseMenu.SetActive(false);
                GameManager.Instance.UpgradesNotAvailable();
                GameManager.Instance.StartCoroutine(GameManager.Instance.SpecialAttack(specialName, typingCode, specialTime, specialCost));
                GameManager.Instance.DisablingHand();
            }
            //if the player right clicks while fighting
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                //we will show the description box
                ShowDescriptionBox();

                //disbaling the hand
                GameManager.Instance.DisablingHand();
            }
        }
        else
        {
            //if the player left clicks while not fighting, we will show the details of the spell
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                ShowDescriptionBox();

                GameManager.Instance.DisablingHand();
            }
        }
    }

    //code to show the description box and assign the corresponding variables
    void ShowDescriptionBox()
    {
        spriteSlot.sprite = specialImage;

        specialNameText.text = specialName;

        specialDescriptionText.text = specialDescription;

        descriptionBox.SetActive(true);
    }


    //putting the hand on the position I want it too, and setting its scale lower
    public void OnPointerEnter(PointerEventData eventData)
    {
        GameObject hand = GameObject.Find("Hand 1");

        hand.transform.localScale = new Vector3(.73f, .73f, .73f);

        hand.transform.position = postion.position;
    }

    //getting the hand out of the way, puff and its scale back to normal
    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.Instance.DisablingHand();
    }


}
