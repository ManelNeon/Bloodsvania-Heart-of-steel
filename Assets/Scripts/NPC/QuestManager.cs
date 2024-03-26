using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    //array containg all the special's slots
    public QuestSlot[] questSlots;

    //adding a special, same requirements as previously, yet this time we do a for loop to see a special slot thats empty and adding the information in, if its empty, debug a specials full
    public void AddQuest(string questName, string questGiver, string questDescription)
    {
        for (int i = 0; i < questSlots.Length; i++)
        {
            if (!questSlots[i].isFull)
            {
                questSlots[i].AddQuest(questName,questGiver,questDescription);
                return;
            }
        }
        Debug.Log("Quests Full");
    }

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
}
