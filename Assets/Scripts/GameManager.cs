using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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

    //getting the buttons that up the skills
    [SerializeField] GameObject skillButtons;

    //getting the player game object
    [SerializeField] GameObject player;

    //getting the crit special effect
    [SerializeField] ParticleSystem critEffect;

    //getting the damage special effect
    [SerializeField] ParticleSystem damageEffect;

    //getting the panel where we inform the player what's happening on screen
    [SerializeField] TextMeshProUGUI attackPanel;

    [SerializeField] GameObject textPrefab;

    [SerializeField] GameObject textPlacement;

    //getting the enemy prefabs
    [SerializeField] GameObject[] enemyPrefabs;

    //gameobject that will store the enemy instantiated in the fight scene
    GameObject enemy;

    //stats from the enemy instantiated
    Enemy enemyStats;

    //cheking if it is or not the player's turn
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
        //if the player clicks escape, activate or deactivate the pause menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenu.activeInHierarchy)
            {
                pauseMenu.SetActive(false);
                UpgradesNotAvailable();
            }
            else if (!pauseMenu.activeInHierarchy)
            {
                pauseMenu.SetActive(true);
                if (player.GetComponent<Player>().skillPoint > 0)
                {
                    skillButtons.SetActive(true);
                }
                ChangeStats();
            }
        } 
    }
    
    //code that checks if the player has any skillpoints, if he hasn't, deactivate the upgrade skill buttons
    public void UpgradesNotAvailable()
    {
        if (player.GetComponent<Player>().skillPoint == 0)
        {
            skillButtons.SetActive(false);
        }
    }
    
    /*Changing the stats on the pause menu, finding all the stats on the main menu and changing the stats, because this ideally only plays when the player
    is on the main menu, no need to reference the objects on the variables */
    public void ChangeStats()
    {
        Player playerStats = player.GetComponent<Player>();

        GameObject.Find("LevelStat").GetComponent<TextMeshProUGUI>().text = "Level - " + playerStats.level; 
        GameObject.Find("AttackStat").GetComponent<TextMeshProUGUI>().text = "Attack - " + playerStats.attack;
        GameObject.Find("HealthStat").GetComponent<TextMeshProUGUI>().text = "Health - " + playerStats.maxHealth;
        GameObject.Find("ManaStat").GetComponent<TextMeshProUGUI>().text = "Blood - " + playerStats.maxMana;
        GameObject.Find("ApptitudeStat").GetComponent<TextMeshProUGUI>().text = "Apptitude - " + playerStats.aptitude;
        GameObject.Find("XpStat").GetComponent<TextMeshProUGUI>().text =  playerStats.xp + " / " + playerStats.xpForLevel;

        if (playerStats.hasSpecialOne)
        {
            GameObject text = Instantiate(textPrefab, textPlacement.transform.position, textPrefab.transform.rotation);

            text.transform.SetParent(textPlacement.transform);

            text.GetComponent<TextMeshProUGUI>().text = ("Beast's Sword");

            text.transform.position = new Vector3(text.transform.position.x - 65, text.transform.position.y - 85, text.transform.position.z);
        }
    }

    //activating the fight scene and deactivating the walk scene
    public void ActivateFightScene(int enemyCode)
    {
        walkScene.SetActive(false); 
        fightScene.SetActive(true);

        //instantiating the enemy, on the coordinates where he's on screen, with the enemycode that every enemyprefab has, then getting the required components
        enemy = Instantiate(enemyPrefabs[enemyCode], new Vector3(44.9f, 2.2f), enemyPrefabs[enemyCode].transform.rotation);
        enemyStats = enemy.GetComponent<Enemy>();

        //checking that its the player turn
        playerTurn = true;
    }

    //activating the walk scene and deactivating the fight scene
    public void ActivateWalkScene()
    {
        fightScene.SetActive(false);
        walkScene.SetActive(true);
    }

    //Ienumerator for when the player attacks the enemy, checking if it is a special, and it's attack code
    public IEnumerator AttackEnemy(bool isSpecial, int attackCode)
    {
        if (playerTurn)
        {
            //getting the playerstats
            Player playerStats = player.GetComponent<Player>();

            //intializing the random multiplier
            int randomMultiplier;

            //intializing the attack value
            int attack;

            //checking if the attack is a special or not
            if (!isSpecial)
            {
                //randomizing the multiplier
                randomMultiplier = Random.Range(1, 6);

                //creating a new attack variable that will multiply with the attack stat from the player
                attack = playerStats.attack * randomMultiplier;

                //changing the text on the attack panel
                attackPanel.text = "Player has attacked for " + attack + " !!!";
            }
            else
            {
                //random multiplier bigger if it is a special
                randomMultiplier = Random.Range(3, 8);

                //multiplying with the aptitude of the player instead of the attack
                attack = playerStats.aptitude * randomMultiplier;

                //checking if it's super effective, also takes the mana away
                if (SuperEffectiveCheck(attackCode))
                {
                    //if it is super effective, multiply by 2
                    attack *= 2;

                    //changing the text on the attack panel
                    attackPanel.text = "IT'S SUPER EFFECTIVE, THE Player has attacked for " + attack + "!!!!!";
                }
                else
                {
                    //changing the text on the attack panel
                    attackPanel.text = "Player has attacked for " + attack + " !!!";
                }
            }

            //the player's turn is false
            playerTurn = false;

            //waiting time for the animation to play (still no animation, just preparing for when I get them)
            yield return new WaitForSeconds(2);

            //playing the speciall effects function
            SpecialEffects(randomMultiplier, true);

            //According to the attack code, an equivalent animation will play, so here you put something like Animator.SetInt(attackCode_i, attackCode);

            //waiting two seconds for the damage to apply for good measure
            yield return new WaitForSeconds(2);

            //applying damage to the enemy
            enemyStats.Damage(attack);

            //waiting just 0.1 seconds so that if the enemy is destroyed the code can recognize it after
            yield return new WaitForSeconds(0.1f);

            //checking if the enemy is null, if it isnt, its the enemy turn and the enemy will attack
            if (enemy != null)
            {
                attackPanel.text = "It's the enemy's turn now.";

                StartCoroutine(EnemyAttack());

                yield break;
            }
            //if the enemy is null, the player will get xp from the enemystats, and then activate the walk scene and breaking the coroutine
            else
            {
                attackPanel.text = "You have defeated the enemy!!";

                yield return new WaitForSeconds(1);

                playerStats.GetXP(enemyStats.xpDrop);

                attackPanel.text = "You got " + enemyStats.xpDrop + " XP!!";

                yield return new WaitForSeconds(1);

                ActivateWalkScene();

                yield break;
            }
        }
        else
        {
            yield break;
        }
    }

    //code that plays when its the enemy turn to attack
    IEnumerator EnemyAttack()
    {
        //wait 1 second for pacing issues
        yield return new WaitForSeconds(1);

        //getting the player stats
        Player playerStats = player.GetComponent<Player>();

        //getting a random multiplier to multiply with the enemies attack
        int randomMultiplier = Random.Range(1, 4);

        int attack = enemyStats.attack * randomMultiplier;

        //showing the player how much damage he did
        attackPanel.text = "The enemy has attacked for " + attack + " !!!";

        //damaging the player
        playerStats.Damage(attack);

        //waiting 2 seconds for the animation
        yield return new WaitForSeconds(2);

        //playing the special effects
        SpecialEffects(randomMultiplier, false);

        yield return new WaitForSeconds(2);

        //its the player turn now after 2 seconds
        playerTurn = true;

        attackPanel.text = "Its your turn now!";

        yield break;
    }
    
    //function that needs the special code, and with a switch case, takes the mana off and returns true if it's super effective or false if it isn't
    bool SuperEffectiveCheck(int specialCode)
    {
        //getting the player's stats
        Player playerStats = player.GetComponent<Player>();

        switch (specialCode)
        {
            case 1:

                //taking the mana off the player
                playerStats.currentMana -= 25;

                //checking if it's super effective
                if (enemyStats.typing == 2 || enemyStats.typing == 3)
                {
                    //if it is returns true
                    return true;
                }

                //if it isnt returns false
                return false;

            case 2:

                playerStats.currentMana -= 20;

                if (enemyStats.typing == 3 || enemyStats.typing == 4)
                {
                    return true;
                }

                return false;

            case 3:

                playerStats.currentMana -= 15;

                if (enemyStats.typing == 4 || enemyStats.typing == 5)
                {
                    return true;
                }

                return false;

            case 4:

                playerStats.currentMana -= 35;

                if (enemyStats.typing == 5 || enemyStats.typing == 1)
                {
                    return true;
                }

                return false;

            case 5:

                playerStats.currentMana -= 20;

                if (enemyStats.typing == 1 || enemyStats.typing == 2)
                {
                    return true;
                }

                return false;

            default:

                return false;
        }
    }

    //special effects function, that will receieve the random multiplier to see if it was a crit, and a bool to see if it is or not the player attacking
    void SpecialEffects(int randomMultiplier, bool isPlayerAttack)
    {
        //if it is the player attacking, the crit effect will play when the multiplier is 5 or more and teleporting the effects to the enemies location
        if (isPlayerAttack)
        {
            critEffect.transform.position = new Vector3(44.9f, 2.2f);

            damageEffect.transform.position = new Vector3(44.9f, 2.2f);

            if (randomMultiplier > 4)
            {
                critEffect.Play();
            }

            damageEffect.Play();
        }
        //if it is the enemy attacking, the crit effect will play when the multiplier is 3 or more and teleport the effects to the players location
        else
        {
            critEffect.transform.position = new Vector3(61.6f, 2.2f);

            damageEffect.transform.position = new Vector3(61.6f, 2.2f);

            if (randomMultiplier > 2)
            {
                critEffect.Play();
            }

            damageEffect.Play();
        }
    }
}
