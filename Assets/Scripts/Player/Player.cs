using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//child of the stats class
public class Player : Stats
{
    //skillPoints
    [HideInInspector] public int skillPoint;

    //player's level, starts at 1
    [HideInInspector] public int level;

    //player's xp
    [HideInInspector] public int xp;

    //player's sprite
    public Sprite playerSprite;

    //player's gold
    [HideInInspector] public int gold;

    //how much xp the player needs to level up
    [HideInInspector] public int xpForLevel;

    //checking if the player is dead
    [HideInInspector] public bool isDead;

    //getting the game over display
    [SerializeField] GameObject gameOverDisplay;

    //getting the game over text
    [SerializeField] TextMeshProUGUI gameOverText;

    //overriden start function
    protected override void Start()
    {
        //getting the start from father class and going with it
        base.Start();

        level = 1;

        //calculating the xp needed for the new level
        NewLevelXP();
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

    public override void Damage(int damage)
    {
        //checking if the damage will put the player below zero
        if (currentHealth - damage > 0)
        {
            currentHealth -= damage;
        }
        //ife he does destroy the enemy
        else
        {
            currentHealth = 0;

            GameManager.Instance.playerTurn = false;
            
            //stopping all coroutines in the game manager so the game stops playing
            GameManager.Instance.StopAllCoroutines();

            //game over sequence
            StartCoroutine(GameOverSequence());
        }
    }

    //the game over sequence
    IEnumerator GameOverSequence()
    {
        //THE PLAYER IS DEAD
        isDead = true;

        //put player death animation here

        yield return new WaitForSeconds(3); //animation time

        //the game over display is activated
        gameOverDisplay.SetActive(true);

        //the text is nothing now
        gameOverText.text = "";

        //the text that will appear when we die
        string text = "You have passed away, go back to the main menu and get your last save file!";

        //the text display 
        foreach (char c in text)
        {
            gameOverText.text += c;
            yield return new WaitForSeconds(GameManager.Instance.textSpeed);
        }

        //stopping the coroutine
        yield break;
    }
}
