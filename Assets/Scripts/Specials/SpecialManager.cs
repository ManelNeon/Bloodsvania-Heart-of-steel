using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialManager : MonoBehaviour
{
    //array containg all the special's slots
    public SpecialSlot[] specialSlots;

    //array containg all the special prefabs
    [SerializeField] Special[] specialsPrefabs;

    //index
    int index;

    //adding a special, we do a for loop to see a special slot thats empty and adding the information in
    public void AddSpecial(int specialID)
    {
        //getting the special
        for (int i = 0; i < specialsPrefabs.Length; i++)
        {
            if (specialsPrefabs[i].specialID == specialID)
            {
                index = i;
            }
        }

        //adding it to the empty spot
        for (int i = 0; i < specialSlots.Length; i++)
        {
            Special currentSpecial = specialsPrefabs[index];
            if (!specialSlots[i].isFull)
            {
                specialSlots[i].AddSpecial(currentSpecial.specialName, currentSpecial.typingCode, currentSpecial.specialTime, currentSpecial.specialImage, currentSpecial.specialDescription, currentSpecial.specialCost, specialID);
                return;
            }
        }
        Debug.Log("Specials Full");
    }

    //for the npc that gives specials, we check if the specials name exists, if it exists the player will not learn a repeated special
    public bool CheckSpecial(string specialName)
    {
        for (int i = 0; i < specialSlots.Length; i++)
        {
            if (specialSlots[i].specialName == specialName)
            {
                return true;
            }
        }

        return false;
    }


    //giving the data manager the corresponding special slot to the index in there
    public SpecialSlot GettingSpecials(int i)
    {
        if (specialSlots[i].isFull)
        {
            return specialSlots[i];
        }
        return null;
    }
}
