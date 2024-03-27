using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataManager : MonoBehaviour
{
    PlayerController playerController;

    Player playerStats;

    InventoryManager inventoryManager;

    QuestManager questManager;

    SpecialManager specialManager;

    private void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        playerStats = GameObject.Find("PlayerStatsHolder").GetComponent<Player>();

        inventoryManager = GameObject.Find("PlayerStatsHolder").GetComponent<InventoryManager>();

        questManager = GameObject.Find("PlayerStatsHolder").GetComponent<QuestManager>();

        specialManager = GameObject.Find("PlayerStatsHolder").GetComponent<SpecialManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            SavePlayerData();
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            DeleteData();
        }
    }

    [System.Serializable]
    class SaveDataPlayer
    {
        public float transformPositionX;

        public float transformPositionY;

        public int gold;

        public int specials;

        public int[] specialID = new int[30];

        public int items;

        public int[] itemsID = new int[30];

        public int[] itemsQuantity = new int[30];

        public int quests;

        public int[] questsID = new int[30];

        public bool[] questCompleted = new bool[30];

        public bool[] questAcceptedSecond = new bool[30];
    }

    public void SavePlayerData()
    {
        string path = Application.persistentDataPath + "/savePlayerData.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);

            SaveDataPlayer data = JsonUtility.FromJson<SaveDataPlayer>(json);

            data.transformPositionX = playerController.transform.position.x;

            json = JsonUtility.ToJson(data);

            File.WriteAllText(Application.persistentDataPath + "/savePlayerData.json", json);
        }
        else
        {
            SaveDataPlayer data = new SaveDataPlayer();

            data.transformPositionX = playerController.transform.position.x;

            data.transformPositionY = playerController.transform.position.y;

            data.gold = playerStats.gold;

            for (int i = 0; i < specialManager.specialSlots.Length; i++)
            {
                if (specialManager.GettingSpecials(i) != null)
                {
                    data.specials++;

                    data.specialID[i] = specialManager.GettingSpecials(i).specialID;
                }
            }

            for (int i = 0; i < inventoryManager.itemSlot.Length; i++)
            {
                if (inventoryManager.GetItemSlot(i) != null)
                {
                    data.items++;

                    data.itemsID[i] = inventoryManager.GetItemSlot(i).itemID;

                    data.itemsQuantity[i] = inventoryManager.GetItemSlot(i).quantity;
                }
            }

            for (int i = 0; i < questManager.questSlots.Length; i++)
            {
                if (questManager.GetQuestSlot(i) != null)
                {
                    data.quests++;

                    data.questsID[i] = questManager.GetQuestSlot(i).questID;

                    if (questManager.GetSideQuestBools(data.questsID[i]) != null)
                    {
                        data.questAcceptedSecond[i] = questManager.GetSideQuestBools(data.questsID[i]).questAcceptedSecond;
                    }
                }
            }

            for (int i = 0; i < questManager.quests.Length; i++)
            {
                if (questManager.GetSideQuestBools(0) != null)
                {
                    data.questsID[i] = questManager.GetSideQuestBools(0).questID;

                    data.questCompleted[i] = questManager.GetSideQuestBools(0).questCompleted;
                }
            }

            string json = JsonUtility.ToJson(data);

            Debug.Log(json);

            File.WriteAllText(Application.persistentDataPath + "/savePlayerData.json", json);
        }
    }

    public void DeleteData()
    {
        string path = Application.persistentDataPath + "/savePlayerData.json";

        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("Deleted");
        }
    }
}