using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//the script for when we enter the stats menu in the UI
public class PlayerPanelStats : MonoBehaviour
{
    //getting the player's stats
    Player playerStats;

    //getting the lace where the players image will be
    [SerializeField] Image playerSprite;

    //place where players name will be
    [SerializeField] TextMeshProUGUI playerName;

    //place where players level will be
    [SerializeField] TextMeshProUGUI playerLevel;

    //place where players experience will be
    [SerializeField] TextMeshProUGUI experienceBar;

    //place where his skillpoints will be
    [SerializeField] TextMeshProUGUI skillPoints;

    //place where the health stat will be
    [SerializeField] TextMeshProUGUI healthStat;

    //place where the blood stat will be
    [SerializeField] TextMeshProUGUI bloodStat;

    //place where the attack stat will be
    [SerializeField] TextMeshProUGUI attackStat;

    //place where the apptitude stat will be
    [SerializeField] TextMeshProUGUI apptitudeStat;

    //array containing the upgrade buttons
    [SerializeField] GameObject[] upgradeButtons;

    //when awaken get the player stats
    private void Awake()
    {
        playerStats = GameObject.Find("PlayerStatsHolder").GetComponent<Player>();
    }

    //on enable change the stats and check if the upgrades are available
    private void OnEnable()
    {
        ChangeStats();

        UpgradesAvailable();
    }

    //changing the stats accordingly
    void ChangeStats()
    {
        playerName.text = playerStats.userName;

        playerLevel.text = "Level " + playerStats.level;

        experienceBar.text = playerStats.xp + "/" + playerStats.xpForLevel;

        skillPoints.text = "Skillpoints : " + playerStats.skillPoint;

        playerSprite.sprite = playerStats.playerSprite;

        healthStat.text = playerStats.currentHealth + "/" + playerStats.maxHealth;

        bloodStat.text = playerStats.currentMana + "/" + playerStats.maxMana;

        attackStat.text = playerStats.attack.ToString();

        apptitudeStat.text = playerStats.aptitude.ToString();
    }

    //checking if the player has skillpoint, if he has the upgrades button will be set to active
    void UpgradesAvailable()
    {
        if (playerStats.GetComponent<Player>().skillPoint == 0)
        {
            for (int i = 0; i < upgradeButtons.Length; i++)
            {
                upgradeButtons[i].SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < upgradeButtons.Length; i++)
            {
                upgradeButtons[i].SetActive(true);
            }
        }
    }

    //the function that upgrades the health, changes the stats, disables the hands and checks if the player still has a skillpoint
    public void UpgradeHealth()
    {
        playerStats.skillPoint--;

        playerStats.maxHealth += 50;

        ChangeStats();

        GameManager.Instance.DisablingHand();

        UpgradesAvailable();
    }

    public void UpgradeBlood()
    {
        playerStats.skillPoint--;

        playerStats.maxMana += 30;

        ChangeStats();

        GameManager.Instance.DisablingHand();

        UpgradesAvailable();
    }

    public void UpgradeAttack()
    {
        playerStats.skillPoint--;

        playerStats.attack += 15;

        ChangeStats();

        GameManager.Instance.DisablingHand();

        UpgradesAvailable();
    }

    public void UpgradeApptitude()
    {
        playerStats.skillPoint--;

        playerStats.aptitude += 25;

        ChangeStats();

        GameManager.Instance.DisablingHand();

        UpgradesAvailable();
    }
}
