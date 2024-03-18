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

    }
}
