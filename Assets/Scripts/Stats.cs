using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//father class for stats, for both enemies and players
public class Stats : MonoBehaviour
{
    //player's or enemies name
    public string userName;

    //player's health stat
    public int maxHealth;

    //player's current health
    [HideInInspector] public int currentHealth;

    //player's "mana" stat
    public int maxMana;

    //player's current "mana"
    public int currentMana;

    //player's attack stat
    public int attack;

    //player's aptitude stat
    public int aptitude;

    protected virtual void Start()
    {
        //setting the player's current health to the max health
        currentHealth = maxHealth;

        //setting the player's mana to the max mana
        currentMana = maxMana;
    }

    //code that plays when i want to damage the player/enemy
    public virtual void Damage(int damage)
    {
        //checking if the damage will put the player below zero
        if (currentHealth- damage > 0)
        {
            currentHealth -= damage;
        }
        //ife he does destroy the enemy
        else
        {
            currentHealth = 0;
            if (gameObject.CompareTag("Enemy"))
            {
                Destroy(gameObject);
            }
        }
    }
}
