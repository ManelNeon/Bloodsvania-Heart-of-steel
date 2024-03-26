using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialManager : MonoBehaviour
{
    //array containg all the special's slots
    public SpecialSlot[] specialSlots;

    //adding a special, same requirements as previously, yet this time we do a for loop to see a special slot thats empty and adding the information in, if its empty, debug a specials full
    public void AddSpecial(string specialName, int specialType, float specialTime, Sprite specialImage, string specialDescription, int specialCost)
    {
        for (int i = 0; i < specialSlots.Length; i++)
        {
            if (!specialSlots[i].isFull)
            {
                specialSlots[i].AddSpecial(specialName, specialType, specialTime, specialImage, specialDescription, specialCost);
                return;
            }
        }
        Debug.Log("Specials Full");
    }

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

    public SpecialSlot GettingSpecials(int i)
    {
        if (specialSlots[i].isFull)
        {
            return specialSlots[i];
        }
        return null;
    }
}
