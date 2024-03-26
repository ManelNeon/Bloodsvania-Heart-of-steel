using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPanelStats : MonoBehaviour
{
    Player playerStats;

    [SerializeField] Image playerSprite;

    [SerializeField] TextMeshProUGUI playerName;

    [SerializeField] TextMeshProUGUI playerLevel;

    [SerializeField] TextMeshProUGUI experienceBar;

    [SerializeField] TextMeshProUGUI skillPoints;

    [SerializeField] TextMeshProUGUI healthStat;

    [SerializeField] TextMeshProUGUI bloodStat;

    [SerializeField] TextMeshProUGUI attackStat;

    [SerializeField] TextMeshProUGUI apptitudeStat;

    [SerializeField] GameObject[] upgradeButtons;

    private void Awake()
    {
        playerStats = GameObject.Find("PlayerStatsHolder").GetComponent<Player>();
    }

    private void OnEnable()
    {
        ChangeStats();

        UpgradesAvailable();
    }

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
