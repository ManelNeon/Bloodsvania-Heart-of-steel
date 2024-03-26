using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Stats
{
    //skillPoints
    [HideInInspector] public int skillPoint;

    //player's level, starts at 1
    [HideInInspector] public int level;

    //player's xp
    [HideInInspector] public int xp;

    [SerializeField] Sprite playerSprite;

    [HideInInspector] public int gold;

    //how much xp the player needs to level up
    [HideInInspector] public int xpForLevel;

    [HideInInspector] public bool hasSpecialOne;

    [HideInInspector] public bool hasSpecialTwo;

    [HideInInspector] public bool hasSpecialThree;

    [HideInInspector] public bool hasSpecialFour;

    [HideInInspector] public bool hasSpecialFive;

    protected override void Start()
    {
        //getting the start from father class and going with it
        base.Start();

        //setting the level at 1
        level = 1;

        //calculating the xp needed for the new level
        NewLevelXP();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LevelUp();
        }
    }

    //getting xp, it's public because will be used outside this code probably
    public void GetXP(int newXP)
    {
        //checking if the addition of the current xp with the new xp is below the required to level up
        if (xp + newXP < xpForLevel)
        {
            //only add the new xp to the current xp
            xp += newXP;
        }
        //checking if the addition of the current xp with the new xp is the same as the required to level up
        else if (xp + newXP == xpForLevel)
        {
            //level up
            LevelUp();

            //putting the xp at zero
            xp = 0;
        }
        //checking if the addition of the current xp with the new xp is above the required to level up
        else if (xp + newXP > xpForLevel)
        {
            //the xp will now be the addition subtracted by the quantity required to level up
            xp = (xp + newXP) - xpForLevel;

            //will level up
            LevelUp();

            //will enter this function
            RepeatLevelUp();
        }
    }

    //function that checks if the xp is still bigget than the xp for level, in case it isnt, return like nothing happened
    void RepeatLevelUp()
    {
        //if it still is bigget than the required for level up
        if (xp > xpForLevel)
        {
            //we subtract it and level up, restarting the function to check if it still is bigger, entering a sort of a for loop
            xp -= xpForLevel;
            LevelUp();
            RepeatLevelUp();
        }
        else
        {
            return;
        }
    }

    //code that calculates the xp required to level up
    void NewLevelXP()
    {
        //if the player's level is 5 or under, the required xp is only the double of the current level
        if (level <= 5)
        {
            xpForLevel = level * 2;
        }
        //if the player's level is higher than 5, the required xp will be three times the current level
        else
        {
            xpForLevel = level * 3;
        }
    }



    //levels up the player and calculates the new xp required to level up, also adds everything to the stats
    void LevelUp()
    {
        level++;
        NewLevelXP();
        attack += 5;
        aptitude += 5;
        maxHealth += 10;
        maxMana += 10;
        skillPoint++;
    }
}
