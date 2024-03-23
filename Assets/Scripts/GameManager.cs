using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
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
    public GameObject pauseMenu;

    //getting the buttons that up the skills
    [SerializeField] GameObject skillButtons;

    //getting the player game object
    [SerializeField] GameObject player;

    //getting the player Stats
    Player playerStats;

    //getting the crit special effect
    [SerializeField] ParticleSystem critEffect;

    //getting the damage special effect
    [SerializeField] ParticleSystem damageEffect;

    //getting the panel where we inform the player what's happening on screen
    [SerializeField] TextMeshProUGUI attackPanel;

    //the display that shows the enemies we're fighting on the fight scene
    [SerializeField] TextMeshProUGUI enemiesDisplay;

    //the display that shows the player's party
    [SerializeField] TextMeshProUGUI playerDisplay;

    //getting the enemy prefabs
    [SerializeField] GameObject[] enemyPrefabs;

    //gameobject that will store the enemy instantiated in the fight scene
    GameObject enemy;

    //stats from the enemy instantiated
    Enemy enemyStats;

    //cheking if it is or not the player's turn
    [HideInInspector] public bool playerTurn;

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

        //getting the player stats from the player
        playerStats = player.GetComponent<Player>();
    }

    void Update()
    {
        //if the player clicks escape, activate or deactivate the pause menu
        if (Input.GetKeyDown(KeyCode.Escape) && walkScene.activeInHierarchy)
        {
            if (pauseMenu.activeInHierarchy)
            {
                //code that deactivates the hand, necessary in case the mouse is on top of a button
                DisablingHand();
                pauseMenu.SetActive(false);
            }
            else if (!pauseMenu.activeInHierarchy)
            {
                pauseMenu.SetActive(true);
                //checking if the upgrades are available
                UpgradesNotAvailable();
                //changing the stats according to the player
                ChangeStats();
            }
        } 
    }

    //code that disables that hand
    public void DisablingHand()
    {
        GameObject hand = GameObject.Find("Hand 1");

        hand.transform.localScale = new Vector3(0.76f, 0.76f, 0.76f);

        hand.transform.position = new Vector3(-694f, -658f, 0);
    }

    //code that checks if the player has any skillpoints, if he hasn't, deactivate the upgrade skill buttons
    public void UpgradesNotAvailable()
    {
        if (player.GetComponent<Player>().skillPoint == 0)
        {
            skillButtons.SetActive(false);
        }
        else
        {
            skillButtons.SetActive(true);
        }
    }
    
    /*Changing the stats on the pause menu, finding all the stats on the main menu and changing the stats, because this ideally only plays when the player
    is on the main menu, no need to reference the objects on the variables */
    public void ChangeStats()
    {
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

        //instantiating the enemy, on the coordinates where he's on screen, with the enemycode that every enemyprefab has, then getting the required components
        enemy = Instantiate(enemyPrefabs[enemyCode], new Vector3(44.9f, 2.2f), enemyPrefabs[enemyCode].transform.rotation);
        enemyStats = enemy.GetComponent<Enemy>();

        //changing the enemies display
        enemiesDisplay.text = "- " + enemyStats.userName;

        //changing the players display text
        InFightChangeStats();

        //checking that its the player turn
        playerTurn = true;
    }

    //code that changes the player's display text
    void InFightChangeStats()
    {
        playerDisplay.text = "- "+ playerStats.userName +"    "+ playerStats.currentHealth +"/" + playerStats.maxHealth + "     " + playerStats.currentMana + "/" + playerStats.maxMana;
    }

    //activating the walk scene and deactivating the fight scene
    public void ActivateWalkScene()
    {
        fightScene.SetActive(false);
        walkScene.SetActive(true);
    }

    //in here we will check the item's code and play the corresponding sequence
    public void UsingItem(int itemCode, string itemName, int effectQuantity)
    {
        switch (itemCode)
        {
            case 1:
                StartCoroutine(HealingItemUsage(itemName, effectQuantity));
                return;

            case 2:
                StartCoroutine(BloodItemUsage(itemName, effectQuantity));
                return;

            case 3:
                StartCoroutine(DamageItemUsage(itemName, effectQuantity));
                return;

            default: 
                break;
        }
    }

    //the healing item code, we need the name of the item and the effect's quantity
    IEnumerator HealingItemUsage(string itemName, int effectQuantity)
    {
        //checking if it's the player turn and the fight scene is active, if it is, use the item
        if (playerTurn && fightScene.activeInHierarchy)
        {
            //changing the text on the attack panel
            attackPanel.text = "Player has used a " + itemName + "!!!";

            playerTurn = false;

            yield return new WaitForSeconds(1.5f);

            if (playerStats.currentHealth + effectQuantity > playerStats.maxHealth)
            {
                //checking the difference
                int difference = playerStats.maxHealth - playerStats.currentHealth;

                playerStats.currentHealth = playerStats.maxHealth;

                attackPanel.text = "Player has healed for " + difference + "!!!";
            }
            else
            {
                playerStats.currentHealth += effectQuantity;

                attackPanel.text = "Player has healed for " + effectQuantity + " !!!";

            }

            //changing the stats so that it shows how many mana he has now
            InFightChangeStats();

            StartCoroutine(EnemyAttack());

            yield break;
        }
        else
        {
            //if it is not on the fight scene, the plyaer can still use the item, PUT IN HERE SOME EFFECT MANEL OF THE FUTURE

            if (playerStats.currentHealth + effectQuantity > playerStats.maxHealth)
            {
                playerStats.currentHealth = playerStats.maxHealth;
            }
            else
            {
                playerStats.currentHealth += effectQuantity;
            }

            yield break;
        }
    }

    //identical to the healing item, only this time it gives blood
    IEnumerator BloodItemUsage(string itemName,int effectQuantity)
    {
        if (playerTurn && fightScene.activeInHierarchy)
        {
            //changing the text on the attack panel
            attackPanel.text = "Player has used a " + itemName + "!!!";

            playerTurn = false;

            yield return new WaitForSeconds(1.5f);

            if (playerStats.currentMana + effectQuantity > playerStats.maxMana)
            {
                int difference = playerStats.maxMana - playerStats.currentMana;

                playerStats.currentMana = playerStats.maxMana;

                attackPanel.text = "Player has recovered blood for " + difference + "!!!";
            }
            else
            {
                playerStats.currentMana += effectQuantity;

                attackPanel.text = "Player has recovered blood for " + effectQuantity +" !!!";

            }

            //changing the stats so that it shows how many mana he has now
            InFightChangeStats();

            StartCoroutine(EnemyAttack());

            yield break;
        }
        else
        {
            if (playerStats.currentMana + effectQuantity > playerStats.maxMana)
            {
                playerStats.currentMana = playerStats.maxMana;
            }
            else
            {
                playerStats.currentMana += effectQuantity;
            }

            yield break;
        }
    }


    //the damaging item cant be used outside of combat, but in combat its a simplified version of the attack enemy code
    IEnumerator DamageItemUsage(string itemName, int effectQuantity)
    {
        if (playerTurn && fightScene.activeInHierarchy)
        {
            //changing the text on the attack panel
            attackPanel.text = "Player has used a " + itemName + "!!!";

            playerTurn = false;

            yield return new WaitForSeconds(1.5f);

            attackPanel.text = "You dealt " + effectQuantity + " to the enemy !!!";

            //we send zero instead of the random multiplier because the item never crits
            SpecialEffects(0, true);

            yield return new WaitForSeconds(1);

            //applying damage to the enemy
            enemyStats.Damage(effectQuantity);

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
            Debug.Log("Can't use damaging item outside of combat.");

            yield break;
        }
    }

    //code that plays when we use a special, we need it's name, it's typing and it's animation Time
    public IEnumerator SpecialAttack(string specialName, int typing, float animationTime, int specialCost)
    {
        if (playerTurn)
        {
            if (specialCost <= playerStats.currentMana)
            {
                //intializing the random multiplier
                int randomMultiplier;

                //intializing the attack value
                int attack;

                //random multiplier bigger if it is a special
                randomMultiplier = Random.Range(5, 8);

                //taking the player's mana
                playerStats.currentMana -= specialCost;

                //multiplying with the aptitude of the player instead of the attack
                attack = playerStats.aptitude * randomMultiplier;

                //changing the text on the attack panel
                attackPanel.text = "Player has used " + specialName + "!!!";

                playerTurn = false;

                yield return new WaitForSeconds(1);

                //checking if it's super effective, also takes the mana away
                if (SuperEffectiveCheck(typing))
                {
                    //if it is super effective, multiply by 2
                    attack *= 2;

                    //changing the text on the attack panel
                    attackPanel.text = specialName + " IS SUPER EFFECTIVE !!!";

                    yield return new WaitForSeconds(1);
                }

                //changing the stats so that it shows how many mana he has now
                InFightChangeStats();

                //how much damage the special did
                attackPanel.text = specialName + " dealt " + attack + " damage !!!";

                //here is where the animation will play

                //waiting time for the animation to play (still no animation, just preparing for when I get them)
                yield return new WaitForSeconds(animationTime);

                SpecialEffects(randomMultiplier, true);

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
                pauseMenu.SetActive(false);
                attackPanel.text = "You need more blood for that spell...";
                
                yield break;
            }
        }
        else
        {
            yield break;
        }
    }

    //Ienumerator for when the player attacks the enemy, checking if it is a special, and it's attack code
    public IEnumerator AttackEnemy()
    {
        if (playerTurn)
        {

            //intializing the random multiplier
            int randomMultiplier;

            //intializing the attack value
            int attack;

            //randomizing the multiplier
            randomMultiplier = Random.Range(1, 6);
            
            //creating a new attack variable that will multiply with the attack stat from the player
            attack = playerStats.attack * randomMultiplier;

            //changing the text on the attack panel
            attackPanel.text = "Player has attacked for " + attack + " !!!";

            //the player's turn is false
            playerTurn = false;

            //Here is where the animation will play

            //waiting time for the animation to play (still no animation, just preparing for when I get them)
            yield return new WaitForSeconds(2);

            //playing the speciall effects function
            SpecialEffects(randomMultiplier, true);

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

        InFightChangeStats();

        yield return new WaitForSeconds(2);

        //its the player turn now after 2 seconds
        playerTurn = true;

        attackPanel.text = "Its your turn now!";

        yield break;
    }
    
    //function that needs the special code, and with a switch case, takes the mana off and returns true if it's super effective or false if it isn't
    bool SuperEffectiveCheck(int specialCode)
    {
        switch (specialCode)
        {
            case 1:

                //checking if it's super effective
                if (enemyStats.typing == 2 || enemyStats.typing == 3)
                {
                    //if it is returns true
                    return true;
                }

                //if it isnt returns false
                return false;

            case 2:

                if (enemyStats.typing == 3 || enemyStats.typing == 4)
                {
                    return true;
                }

                return false;

            case 3:

                if (enemyStats.typing == 4 || enemyStats.typing == 5)
                {
                    return true;
                }

                return false;

            case 4:

                if (enemyStats.typing == 5 || enemyStats.typing == 1)
                {
                    return true;
                }

                return false;

            case 5:

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
