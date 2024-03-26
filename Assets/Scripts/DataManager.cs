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

        public int quests;
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