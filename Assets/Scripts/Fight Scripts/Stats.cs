using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Stats : MonoBehaviour
{
    //player's health stat
    public int maxHealth;

    //player's current health
    int currentHealth;

    //player's "mana" stat
    public int maxMana;

    //player's current "mana"
    int currentMana;

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

    public void Damage(int damage)
    {
        if (currentHealth- damage > 0)
        {
            currentHealth -= damage;
        }
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
