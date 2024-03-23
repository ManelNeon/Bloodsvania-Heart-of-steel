using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Special : MonoBehaviour
{
    //the special's name
    [SerializeField] string specialName;

    //the typing Code
    [SerializeField] int typingCode;

    //time needed to play the special's animation
    [SerializeField] float specialTime;

    //special's Description
    [SerializeField] string specialDescription;

    [SerializeField] int specialCost;

    //special's image
    [TextArea] [SerializeField] Sprite specialImage;

    //code to call when we want to learn the new special
    void LearnSpecial()
    {
        SpecialManager manager = GameObject.Find("PlayerStatsHolder").GetComponent<SpecialManager>();

        manager.AddSpecial(specialName, typingCode, specialTime, specialImage, specialDescription, specialCost);
    }
}
