using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    //array containg all the special's slots
    public QuestSlot[] questSlots;

    //array that contains all the npcs that have side quests
    public NPCSideQuest[] quests;

    //adding a special, same requirements as previously, yet this time we do a for loop to see a special slot thats empty and adding the information in, if its empty, debug a specials full
    public void AddQuest(string questName, string questGiver, string questDescription, int questID)
    {
        for (int i = 0; i < questSlots.Length; i++)
        {
            if (!questSlots[i].isFull)
            {
                questSlots[i].AddQuest(questName,questGiver,questDescription, questID);
                return;
            }
        }
        Debug.Log("Quests Full");
    }

    //completing the side quest, getting a reward and starting the side quest complete function in the game manager
    public void CompleteQuest(int questReward, string questName)
    {
        for (int i = 0; i < questSlots.Length; i++)
        {
            if (questName == questSlots[i].questName)
            {
                questSlots[i].CompleteQuest(questReward);

                GameManager.Instance.SideQuestComplete(questName, questReward);

                return;
            }
        }
    }

    //getting the quest slot in the data manager
    public QuestSlot GetQuestSlot(int i)
    {
        if (questSlots[i].isFull)
        {
            return questSlots[i];
        }
        return null;
    }

    //getting the npc with the quest bools in the data manager
    public NPCSideQuest GetSideQuestBools(int questID)
    {
        for (int i = 0; i < quests.Length; i++)
        {
            //if the id is the same we return the quest
            if (quests[i].questID == questID)
            {
                return quests[i];
            }
            //in the data manager we send a zero to see if the any npc has the quest completed, if he has he will be stored as done
            if (quests[i].questCompleted)
            {
                return quests[i];
            }
        }
        return null;
    }

}
