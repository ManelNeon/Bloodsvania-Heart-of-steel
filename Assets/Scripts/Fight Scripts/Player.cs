using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    //player's health stat
    [SerializeField] int maxHealth;

    //player's current health
    int currentHealth;

    //player's "mana" stat
    [SerializeField] int maxMana;

    //player's current "mana"
    int currentMana;

    //player's attack stat
    [SerializeField] int attack;

    //player's aptitude stat
    [SerializeField] int aptitude;

    //player's level, starts at 1
    [SerializeField] int level;

    //player's xp
    [SerializeField] int xp;

    //how much xp the player needs to level up
    int xpForLevel;

    void Start()
    {
        //setting the player's current health to the max health
        currentHealth = maxHealth;

        //setting the player's mana to the max mana
        currentMana = maxMana;
    }   

    //getting xp, it's public because will be used outside this code probably
    public void GetXP(int newXP)
    {
        //temporary code to establish the quantity needed to level up
        NewLevelXP();

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

    //levels up the player and calculates the new xp required to level up
    void LevelUp()
    {
        level++;
        NewLevelXP();
    }

}
