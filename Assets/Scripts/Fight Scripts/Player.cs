using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] int maxHealth;

    int currentHealth;

    [SerializeField] int maxMana;

    int currentMana;

    void Start()
    {
        currentHealth = maxHealth;

        currentMana = maxMana;
    }   

}
