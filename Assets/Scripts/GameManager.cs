using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // ---- Game Instance ---- //

    // creating a static Instance
    public static GameManager Instance;

    // ---- Game Scenes ---- //

    //getting the fight scene
    [SerializeField] GameObject fightScene;

    //getting the walk scene
    [SerializeField] GameObject walkScene;

    // ---- In Game Necessities ---- //

    //getting the pause menu
    [SerializeField] GameObject pauseMenu;

    [SerializeField] TextMeshProUGUI warningDisplayText;

    [SerializeField] GameObject warningDisplay;

    //getting the buttonManager
    [SerializeField] ButtonManager buttonManager;

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

    //the display that shows the player's party
    [SerializeField] TextMeshProUGUI playerNameDisplay;

    [SerializeField] TextMeshProUGUI playerHPDisplay;

    [SerializeField] TextMeshProUGUI playerBloodDisplay;

    //the display that shows the enemies we're fighting on the fight scene
    [SerializeField] TextMeshProUGUI[] enemiesDisplayName;

    [SerializeField] TextMeshProUGUI[] enemiesDisplayType;

    //getting the enemy prefabs
    [SerializeField] GameObject[] enemyPrefabs;

    //gameobject that will store the enemy instantiated in the fight scene
    GameObject[] enemy = new GameObject[4];

    //the position the enemies can take
    [SerializeField] Transform[] enemyPositions;

    //stats from the enemy instantiated
    Enemy[] enemyStats = new Enemy[4];

    int enemyCount;

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
            if (!pauseMenu.activeInHierarchy)
            {
                pauseMenu.SetActive(true);
            }
            else if (pauseMenu.activeInHierarchy)
            {
                buttonManager.ResumeGame();
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

    //activating the walk scene and deactivating the fight scene
    public void ActivateWalkScene()
    {
        fightScene.SetActive(false);
        walkScene.SetActive(true);
    }

    //activating the fight scene and deactivating the walk scene
    public void ActivateFightScene(int enemyCode, int enemyCount)
    {
        buttonManager.ResumeGame();
        fightScene.SetActive(true);

        this.enemyCount = enemyCount;

        walkScene.SetActive(false);

        for (int i = 0; i < this.enemyCount; i++)
        {
            //instantiating the enemy, on the coordinates where he's on screen, with the enemycode that every enemyprefab has, then getting the required components
            enemy[i] = Instantiate(enemyPrefabs[enemyCode], enemyPositions[i].position, enemyPrefabs[enemyCode].transform.rotation);
            enemyStats[i] = enemy[i].GetComponent<Enemy>();

            //changing the enemies display

            int numberEnemy = i + 1;
            enemiesDisplayName[i].text = "- " + enemyStats[i].userName + " " + numberEnemy;
            string type = Type(enemyStats[i].typing);
            enemiesDisplayType[i].text = type;
            enemiesDisplayName[i].gameObject.SetActive(true);
        }

        attackPanel.text = "It's your turn now.";

        //changing the players display text
        InFightChangeStats();

        //checking that its the player turn
        playerTurn = true;
    }

    string Type(int typeCode)
    {
        switch (typeCode)
        {
            case 1:
                return "Savage";

            case 2:
                return "Machine";

            case 3:
                return "Human";

            case 4:
                return "Fulgurite";

            case 5:
                return "Nature";
            default:
                return null;
        }
    }

    //code that changes the player's display text
    void InFightChangeStats()
    {
        playerNameDisplay.text = playerStats.userName;

        playerHPDisplay.text = playerStats.currentHealth + "/" + playerStats.maxHealth;

        playerBloodDisplay.text = playerStats.currentMana + "/" + playerStats.maxMana;
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
                int difference = playerStats.maxHealth - playerStats.currentHealth;

                playerStats.currentHealth = playerStats.maxHealth;

                warningDisplay.SetActive(true);

                warningDisplayText.text = "You have healed for " + difference + "!!!";
            }
            else
            {
                playerStats.currentHealth += effectQuantity;

                warningDisplay.SetActive(true);

                warningDisplayText.text = "You have healed for " + effectQuantity + " !!!";
            }

            yield return new WaitForSeconds(3);

            warningDisplay.SetActive(false);

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

            //temporary implementation, because I Know that for now there's always only one enemy, no need to make for cycles in here
            int i = 0;

            //applying damage to enemies
            enemyStats[i].Damage(effectQuantity);
            

            //waiting just 0.1 seconds so that if the enemy is destroyed the code can recognize it after
            yield return new WaitForSeconds(0.1f);

            //checking if the enemy is null, if it isnt, its the enemy turn and the enemy will attack
            if (enemy[i] != null)
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

                playerStats.GetXP(enemyStats[i].xpDrop);

                attackPanel.text = "You got " + enemyStats[i].xpDrop + " XP!!";

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

                //temporary
                int i = 0;

                int checkSuperEffect = SuperEffectiveCheck(typing, i);

                //checking if it's super effective
                if (checkSuperEffect == 1)
                {
                    //if it is super effective, multiply by 2
                    attack *= 2;

                    //changing the text on the attack panel
                    attackPanel.text = specialName + " IS SUPER EFFECTIVE !!!";

                    yield return new WaitForSeconds(1);
                }
                else if (checkSuperEffect == 2)
                {
                    //if it is not super effective, divide by 2
                    attack /= 2;

                    //changing the text on the attack panel
                    attackPanel.text = specialName + " is not very effective.... !!!";

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
                enemyStats[i].Damage(attack);

                //waiting just 0.1 seconds so that if the enemy is destroyed the code can recognize it after
                yield return new WaitForSeconds(0.1f);

                //checking if the enemy is null, if it isnt, its the enemy turn and the enemy will attack
                if (enemy[i] != null)
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

                    playerStats.GetXP(enemyStats[i].xpDrop);

                    attackPanel.text = "You got " + enemyStats[i].xpDrop + " XP!!";

                    yield return new WaitForSeconds(1);

                    ActivateWalkScene();

                    yield break;
                }
            }
            else
            {
                buttonManager.ResumeGame();

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

            //temporary
            int i = 0;

            //applying damage to the enemy
            enemyStats[i].Damage(attack);

            //waiting just 0.1 seconds so that if the enemy is destroyed the code can recognize it after
            yield return new WaitForSeconds(0.1f);

            //checking if the enemy is null, if it isnt, its the enemy turn and the enemy will attack
            if (enemy[i] != null)
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

                playerStats.GetXP(enemyStats[i].xpDrop);

                attackPanel.text = "You got " + enemyStats[i].xpDrop + " XP!!";

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

        //temporary
        int i = 0;

        int attack = enemyStats[i].attack * randomMultiplier;

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
    int SuperEffectiveCheck(int specialCode, int i)
    {
        switch (specialCode)
        {
            case 1:

                //checking if it's super effective
                if (enemyStats[i].typing == 2 || enemyStats[i].typing == 3)
                {
                    //if it is returns 1
                    return 1;
                }
                if (enemyStats[i].typing == 5 || enemyStats[i].typing == 4)
                {
                    //if it is not very effective return 2
                    return 2;
                }
                //else return 3
                return 3;

            case 2:

                //checking if it's super effective
                if (enemyStats[i].typing == 3 || enemyStats[i].typing == 4)
                {
                    //if it is returns 1
                    return 1;
                }
                if (enemyStats[i].typing == 1 || enemyStats[i].typing == 5)
                {
                    //if it is not very effective return 2
                    return 2;
                }
                //else return 3
                return 3;

            case 3:

                //checking if it's super effective
                if (enemyStats[i].typing == 4 || enemyStats[i].typing == 5)
                {
                    //if it is returns 1
                    return 1;
                }
                if (enemyStats[i].typing == 1 || enemyStats[i].typing == 2)
                {
                    //if it is not very effective return 2
                    return 2;
                }
                //else return 3
                return 3;

            case 4:

                //checking if it's super effective
                if (enemyStats[i].typing == 5 || enemyStats[i].typing == 1)
                {
                    //if it is returns 1
                    return 1;
                }
                if (enemyStats[i].typing == 2 || enemyStats[i].typing == 3)
                {
                    //if it is not very effective return 2
                    return 2;
                }
                //else return 3
                return 3;

            case 5:

                //checking if it's super effective
                if (enemyStats[i].typing == 1 || enemyStats[i].typing == 2)
                {
                    //if it is returns 1
                    return 1;
                }
                if (enemyStats[i].typing == 3 || enemyStats[i].typing == 4)
                {
                    //if it is not very effective return 2
                    return 2;
                }
                //else return 3
                return 3;

            default:

                return 3;
        }
    }

    //special effects function, that will receieve the random multiplier to see if it was a crit, and a bool to see if it is or not the player attacking
    void SpecialEffects(int randomMultiplier, bool isPlayerAttack)
    {
        //if it is the player attacking, the crit effect will play when the multiplier is 5 or more and teleporting the effects to the enemies location
        if (isPlayerAttack)
        {
            for (int i = 0; i < enemyCount; i++)
            {
                critEffect.transform.position = enemyPositions[i].position;

                damageEffect.transform.position = enemyPositions[i].position;
            }

            if (randomMultiplier > 4)
            {
                critEffect.Play();
            }

            damageEffect.Play();
        }
        //if it is the enemy attacking, the crit effect will play when the multiplier is 3 or more and teleport the effects to the players location
        else
        {
            critEffect.transform.position = new Vector3(60.56f, 1.05f);

            damageEffect.transform.position = new Vector3(60.56f, 1.05f);

            if (randomMultiplier > 2)
            {
                critEffect.Play();
            }

            damageEffect.Play();
        }
    }
}
