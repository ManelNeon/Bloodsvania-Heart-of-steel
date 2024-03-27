using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Special : MonoBehaviour
{
    //the special's name
    public string specialName;

    //the typing Code
    public int typingCode;

    //time needed to play the special's animation
    public float specialTime;

    //special's Description
    [TextArea] public string specialDescription;

    public int specialCost;

    public int specialID;

    //special's image
    public Sprite specialImage;

    //code to call when we want to learn the new special
    public void LearnSpecial()
    {
        SpecialManager manager = GameObject.Find("PlayerStatsHolder").GetComponent<SpecialManager>();

        manager.AddSpecial(specialID);
    }
}
