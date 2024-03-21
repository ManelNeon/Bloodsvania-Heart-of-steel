using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR;

//code for the special Slots on the UI, has a clicker, enter and exit event
public class SpecialSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    //the special's name for the UI
    string specialName;

    //the typing Code, for the UI
    int typingCode;

    //the special code for the attack 
    int specialCode;

    //the typing for the UI
    string typing;

    //checking if the slot is full
    [HideInInspector] public bool isFull;

    //text for the special
    [SerializeField] TMP_Text specialText;

    //position to where the pointer will go
    [SerializeField] Transform postion;

    //code that adds a special, requires the name of the special, it's typing and special code
    public void AddSpecial(string specialName, int typingCode, int specialCode)
    {
        //assigning the special's data to the spots
        this.specialName = specialName;

        this.typingCode = typingCode;

        this.specialCode = specialCode;

        //it is now a full slot
        isFull = true;

        //assigning the type to the typing
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

    //On click event, if its the player's turn, so its on combat, if left click, attack, if right click see what that special does, if not the player turn, not in combat, just see what the special does
    public void OnPointerClick(PointerEventData eventData)
    {
        if (GameManager.Instance.playerTurn)
        {
            if(eventData.button == PointerEventData.InputButton.Left)
            {
                GameManager.Instance.pauseMenu.SetActive(false);
                GameManager.Instance.UpgradesNotAvailable();
                GameManager.Instance.StartCoroutine(GameManager.Instance.AttackEnemy(true, specialCode));
            }
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                Debug.Log("Show What the Special Does");
            }
        }
        else
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                Debug.Log("Show What the Special Does");
            }
        }
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
        GameObject hand = GameObject.Find("Hand 1");

        hand.transform.localScale = new Vector3(1, 1, 1);

        hand.transform.position = new Vector3(-694f, -658f, 0);
    }
}
