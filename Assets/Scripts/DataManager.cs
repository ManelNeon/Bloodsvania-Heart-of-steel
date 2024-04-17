using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

// the holy mother of data, the data manager, responsible for loading and saving the player data
public class DataManager : MonoBehaviour
{
    //creating the instance, so there's only one data manager
    public static DataManager Instance; 

    //in the inspector we get the player controller so we can save the player's position
    [SerializeField] PlayerController playerController;

    //in the inspector we get the player stats so we can get his gold
    [SerializeField] Player playerStats;

    //in the inspector we get the inventory manager so we can get the items he has
    [SerializeField] InventoryManager inventoryManager;

    //in the inspector we get the quest manager so we can get the quests he has
    [SerializeField] QuestManager questManager;

    //in the inspector we get the specials manager so we can get the specials he has
    [SerializeField] SpecialManager specialManager;

    [SerializeField] GameManager gameManager;

    //in the start function we simply identify the instance
    private void Start()
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

    //debug only, testing
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            SavePlayerData();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadData();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            string path = Application.persistentDataPath + "/savePlayerData.json";

            if (File.Exists(path))
            {
                Debug.Log("2");
                File.Delete(path);
            }
        }
    }

    //the serializable class that contains the data we need to save
    [System.Serializable]
    class SaveDataPlayer
    {
        //we save the x position of the player
        public float transformPositionX;

        //we save the y position of the player
        public float transformPositionY;

        //we save how much gold he has
        public int gold;

        public int level;

        public int healthStat;

        public int manaStat;

        public int attackStat;

        public int apptitudeStat;

        public bool isTutorial;

        //we save how many specials he has
        public int specials;

        //we get the specials ID in a array
        public int[] specialID = new int[30];

        //we save how many items we have
        public int items;

        //we get the items ID in a array
        public int[] itemsID = new int[30];

        //the corresponding item quantiy in another array
        public int[] itemsQuantity = new int[30];

        //we save how many quests he has
        public int quests;

        //we get the quests ID
        public int[] questsID = new int[30];

        //we get a bool corresponding to each NPC that gives a sidequest, checking if they have their quest completed
        public bool[] questCompleted = new bool[30];
    }

    //in here we save the data
    public void SavePlayerData()
    {
        //firstly we create a new save data
        SaveDataPlayer data = new SaveDataPlayer();

        //we get the players tranform position in the x and y
        data.transformPositionX = playerController.transform.position.x;

        data.transformPositionY = playerController.transform.position.y;

        //we get the player's gold
        data.gold = playerStats.gold;

        data.level = playerStats.level;

        data.healthStat = playerStats.maxHealth;

        data.manaStat = playerStats.maxMana;

        data.attackStat = playerStats.attack;

        data.apptitudeStat = playerStats.aptitude;

        data.isTutorial = gameManager.isTutorial;

        //we check the special manager's special slots, and see if they're full, if they are we store their special ID
        for (int i = 0; i < specialManager.specialSlots.Length; i++)
        {
            if (specialManager.GettingSpecials(i) != null)
            {
                data.specials++;

                data.specialID[i] = specialManager.GettingSpecials(i).specialID;
            }
        }

        //we check the inventory manager's item slots, and see if they're full, if they are we store their itemID and their quantity
        for (int i = 0; i < inventoryManager.itemSlot.Length; i++)
        {
            if (inventoryManager.GetItemSlot(i) != null)
            {
                data.items++;

                data.itemsID[i] = inventoryManager.GetItemSlot(i).itemID;

                data.itemsQuantity[i] = inventoryManager.GetItemSlot(i).quantity;
            }
        }

        //for the quests its a bit more complpicated, the beggining is the same, checking if the quest slot is full, if it is we get the quest ID
        for (int i = 0; i < questManager.questSlots.Length; i++)
        {
            if (questManager.GetQuestSlot(i) != null)
            {
                data.quests++;

                data.questsID[i] = questManager.GetQuestSlot(i).questID;
            }
        }

        //here we do the same thing, yet we look for the quests length, we put a zero in (because no questID is zero) and we store the quest's ID for completion and the bool for completion
        for (int i = 0; i < questManager.quests.Length; i++)
        {
            if (questManager.GetSideQuestBools(0) != null)
            {
                data.questsID[i] = questManager.GetSideQuestBools(0).questID;

                data.questCompleted[i] = questManager.GetSideQuestBools(0).questCompleted;
            }
        }

        //finally we turn the data into a json
        string json = JsonUtility.ToJson(data);

        Debug.Log(json);

        string path = Application.persistentDataPath + "/savePlayerData.json";

        if (File.Exists(path))
        {
            File.Delete(path);
        }

        //and write the json on a json File
        File.WriteAllText(path, json);
    }

    //in here we will load the data
    public void LoadData()
    {
        string path = Application.persistentDataPath + "/savePlayerData.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);

            SaveDataPlayer data = JsonUtility.FromJson<SaveDataPlayer>(json);

            playerController.transform.position = new Vector3(data.transformPositionX, data.transformPositionY, 0);

            playerStats.gold = data.gold;

            playerStats.maxHealth = data.healthStat;

            playerStats.maxMana = data.manaStat;

            playerStats.attack = data.attackStat;

            playerStats.aptitude = data.apptitudeStat;

            gameManager.isTutorial = data.isTutorial;

            if (data.specials != 0)
            {
                for (int i = 0; i < data.specials; i++)
                {
                    specialManager.AddSpecial(data.specialID[i]);
                }
            }

            if (data.items != 0)
            {
                for (int i = 0; i < data.items; i++)
                {
                    inventoryManager.AddItem(data.itemsID[i], data.itemsQuantity[i]);
                }
            }
            
            if (data.quests != 0)
            {
                for (int i = 0; i < data.quests; i++)
                {
                    questManager.AddQuest(data.questsID[i]);

                    questManager.GetSideQuestBools(data.questsID[i]).questAcceptedSecond = true;
                }
            }

            for (int i = 0; i < data.questCompleted.Length; i++)
            {
                if (data.questCompleted[i])
                {
                    questManager.CompleteQuestData(data.questsID[i]);
                }
            }

            json = JsonUtility.ToJson(data);

            Debug.Log(json);

            gameManager.gameObject.SetActive(true);
        }
    }
}