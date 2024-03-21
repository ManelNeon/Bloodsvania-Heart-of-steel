using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialManager : MonoBehaviour
{
    //array containg all the special's slots
    public SpecialSlot[] specialSlots;

    void Update()
    {
        //code just to check if it was working, debug only
        if (Input.GetKeyDown(KeyCode.Z))
        {
            AddSpecial("Beast's Sword", 1, 1);
        }
    }

    //adding a special, same requirements as previously, yet this time we do a for loop to see a special slot thats empty and adding the information in, if its empty, debug a specials full
    public void AddSpecial(string specialName, int specialType, int specialCode)
    {
        for (int i = 0; i < specialSlots.Length; i++)
        {
            if (!specialSlots[i].isFull)
            {
                specialSlots[i].AddSpecial(specialName, specialType, specialCode);
                return;
            }
        }
        Debug.Log("Specials Full");
    }
}
