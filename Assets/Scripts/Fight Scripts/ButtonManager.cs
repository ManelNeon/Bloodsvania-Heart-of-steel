using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script that contains all of the button functionality, will get big very fast
public class ButtonManager : MonoBehaviour
{
    //getting the player script
    [SerializeField] Player player;

    //the box with the special description
    [SerializeField] GameObject specialDescription;

    //the box with the item description
    [SerializeField] GameObject itemDescription;

    //the pause ui description
    [SerializeField] GameObject pauseUI;

    //disabling the pause description box when pressing back
    public void DisablePauseUI()
    {
        GameManager.Instance.DisablingHand();
        pauseUI.SetActive(false);
    }

    //disabling the special description box when pressing back
    public void DisableSpecialDescription()
    {
        GameManager.Instance.DisablingHand();
        specialDescription.SetActive(false);
    }

    //disabling the item description box when pressing back
    public void DisableItemDescription()
    {
        GameManager.Instance.DisablingHand();
        itemDescription.SetActive(false);
    }


    //code that plays when you select the plus button when you have a skillpoint
    public void AttackPlus()
    {
        //adding 5 to the attack
        player.attack += 5;

        //taking off the skillpoint
        player.skillPoint--;

        //checking if there's any more skillpoints, if there is, dont deactivate the plus buttons, if there is, deactivate them
        GameManager.Instance.UpgradesNotAvailable();

        //changing the stats on the menu
        GameManager.Instance.ChangeStats();

        GameManager.Instance.DisablingHand();
    }

    //code that plays when you select the plus button next to health
    public void HealthPlus()
    {
        player.maxHealth += 10;
        player.skillPoint--;
        GameManager.Instance.UpgradesNotAvailable();
        GameManager.Instance.ChangeStats();
        GameManager.Instance.DisablingHand();
    }

    //code that plays when you select the plus button next to mana
    public void ManaPlus()
    {
        player.maxMana += 10;
        player.skillPoint--;
        GameManager.Instance.UpgradesNotAvailable();
        GameManager.Instance.DisablingHand();
        GameManager.Instance.ChangeStats();
    }

    //code that plays when you select the plus button next to apptitude
    public void AppPlus()
    {
        player.aptitude += 5;
        player.skillPoint--;
        GameManager.Instance.UpgradesNotAvailable();
        GameManager.Instance.ChangeStats();
        GameManager.Instance.DisablingHand();
    }

    //code that plays when you select the items
    public void ItemsSelected()
    {
        if (GameManager.Instance.playerTurn)
        {
            //activating the UI that contains the items
            pauseUI.SetActive(true);
            GameManager.Instance.ChangeStats();
        }
    }

    //code that plays when you select the specials
    public void SpecialsSelected()
    {
        if (GameManager.Instance.playerTurn)
        {
            //activating the UI that contains the specials
            pauseUI.SetActive(true);
            GameManager.Instance.ChangeStats();
        }
    }

    //when we select the attack on the attack screen this plays, sets the UI to inavctive in case the player selected specials but didnt commit to it
    public void AttackSelected()
    {
        if (GameManager.Instance.playerTurn)
        {
            pauseUI.SetActive(false);
            //starting the coroutine for the player attacking the enemy
            GameManager.Instance.StartCoroutine(GameManager.Instance.AttackEnemy());
        }
    }
}
