using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // creating a static Instance
    public static GameManager Instance;

    //getting the fight scene
    [SerializeField] GameObject fightScene;

    //getting the walk scene
    [SerializeField] GameObject walkScene;

    //getting the pause menu
    [SerializeField] GameObject pauseMenu;

    [SerializeField] GameObject player;

    [SerializeField] ParticleSystem critEffect;

    [SerializeField] ParticleSystem damageEffect;

    [SerializeField] GameObject[] enemyPrefabs;

    Stats enemy;

    bool playerTurn;

    // Start is called before the first frame update
    void Start()
    {
        //checking if there's other Instance
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        //if there isnt, this is the Instance
        Instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenu.activeInHierarchy)
            {
                pauseMenu.SetActive(false);
            }
            else if (!pauseMenu.activeInHierarchy)
            {
                pauseMenu.SetActive(true);
                ChangeStats();
            }
        } 
    }

    void ChangeStats()
    {
        Player playerStats = player.GetComponent<Player>();

        GameObject.Find("LevelStat").GetComponent<TextMeshProUGUI>().text = "Level - " + playerStats.level; 
        GameObject.Find("AttackStat").GetComponent<TextMeshProUGUI>().text = "Attack - " + playerStats.attack;
        GameObject.Find("HealthStat").GetComponent<TextMeshProUGUI>().text = "Health - " + playerStats.maxHealth;
        GameObject.Find("ManaStat").GetComponent<TextMeshProUGUI>().text = "Blood - " + playerStats.maxMana;
        GameObject.Find("ApptitudeStat").GetComponent<TextMeshProUGUI>().text = "Apptitude - " + playerStats.aptitude;
        GameObject.Find("XpStat").GetComponent<TextMeshProUGUI>().text =  playerStats.xp + " / " + playerStats.xpForLevel;
    }

    //activating the fight scene and deactivating the walk scene
    public void ActivateFightScene(int enemyCode)
    {
        walkScene.SetActive(false); 
        fightScene.SetActive(true);
        enemy = Instantiate(enemyPrefabs[enemyCode], new Vector3(44.9f, 2.2f), enemyPrefabs[enemyCode].transform.rotation).GetComponent<Stats>();
        playerTurn = true;
    }

    //activating the walk scene and deactivating the fight scene
    public void ActivateWalkScene()
    {
        fightScene.SetActive(false);
        walkScene.SetActive(true);
    }

    public void AttackEnemy()
    {
        if (playerTurn)
        {
            Player playerStats = player.GetComponent<Player>();

            int randomMultiplier = Random.Range(1, 6);
            Debug.Log(randomMultiplier);
            if (randomMultiplier > 4)
            {
                critEffect.Play();
            }
            int attack = playerStats.attack * randomMultiplier;

            damageEffect.Play();
            enemy.Damage(attack);
            playerTurn = false;
            EnemyAttack();
        }
    }

    public void EnemyAttack()
    {
        if (!playerTurn)
        {
            Player playerStats = player.GetComponent<Player>();

            int randomMultiplier = Random.Range(1, 6);
            Debug.Log(randomMultiplier);
            if (randomMultiplier > 4)
            {
                critEffect.Play();
            }

            int attack = enemy.attack * randomMultiplier;

            damageEffect.Play();
            playerStats.Damage(attack);
            playerTurn = true;
        }
    }


}
