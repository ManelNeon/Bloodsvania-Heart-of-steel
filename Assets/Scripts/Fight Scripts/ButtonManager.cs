using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script that contains all of the button functionality, will get big very fast
public class ButtonManager : MonoBehaviour
{
    //getting the items game object
    [SerializeField] GameObject items;

    //getting the specials game object
    [SerializeField] GameObject specials;

    //getting the player script
    [SerializeField] Player player;
 
    //code that plays when you select the specials
    public void SpecialsSelected()
    {
        //checks if the items are active, in case they are deactivate them
        if (items.activeInHierarchy)
        {
            items.SetActive(false);
        }

        //activate specials
        specials.SetActive(true);
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
    }

    //code that plays when you select the plus button next to health
    public void HealthPlus()
    {
        player.maxHealth += 10;
        player.skillPoint--;
        GameManager.Instance.UpgradesNotAvailable();
        GameManager.Instance.ChangeStats();
    }

    //code that plays when you select the plus button next to mana
    public void ManaPlus()
    {
        player.maxMana += 10;
        player.skillPoint--;
        GameManager.Instance.UpgradesNotAvailable();
        GameManager.Instance.ChangeStats();
    }

    //code that plays when you select the plus button next to apptitude
    public void AppPlus()
    {
        player.aptitude += 5;
        player.skillPoint--;
        GameManager.Instance.UpgradesNotAvailable();
        GameManager.Instance.ChangeStats();
    }

    //code that plays when you select the items
    public void ItemsSelected()
    {
        //checks if specials are active, in case they are deactivate them
        if (specials.activeInHierarchy)
        {
            specials.SetActive(false);
        }

        //activate items
        items.SetActive(true);
    }

    //code to play when selecting attack
    public void AttackSelected()
    {
        //starting the coroutine for the player attacking the enemy
        StartCoroutine(GameManager.Instance.AttackEnemy());
    }
}
