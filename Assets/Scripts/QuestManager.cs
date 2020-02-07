using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour {

    static Dictionary<string, FlagData> questFlags = new Dictionary<string, FlagData>(); //quest flags that exist within the scene
    public static List<Quest> activeQuests = new List<Quest>(); // active quests

    private void Start()
    {
        PopulateQuestFlags();
    }

    private void PopulateQuestFlags()
    {
        Quest[] allQuests = FindObjectsOfType<Quest>();
        foreach (Quest q in allQuests)
        {
            foreach (KeyValuePair<string, FlagData> flag in q.questRequirements)
            {
                questFlags.Add(flag.Key, flag.Value);
            }
        }
    }

    public static void AddQuest(Quest questToAdd)
    {
        activeQuests.Add(questToAdd);
        questToAdd.CheckIfComplete();
        print("Accepted quest: '" + questToAdd.questInfo.name + "'");
    }
    /*public static void RemoveQuest(Quest questToRemove)
    {
        activeQuests.Remove(questToRemove);
        print("Quest: '" + questToRemove.questInfo.name + "' is complete!");
    }*/

    public static void UpdateQuestFlag(string flagName)
    {
        if(questFlags[flagName] != null)
        {
            IncrementFlag(questFlags[flagName]);
            print("incremented " + questFlags[flagName]);
        }
        else
        {
            print("There is no quest flag called " + flagName);
        }
        for(int i = activeQuests.Count - 1; i >= 0; i--)
        {
            if (!activeQuests[i].complete)
            {
                print("alkdjajdf" + activeQuests[i]);
                activeQuests[i].CheckIfComplete();
            }
        }
    }

    static void IncrementFlag(FlagData data)
    {
        bool prerequisitesComplete = CheckPrerequisiteQuests(data);
        if (prerequisitesComplete)
        {
            if (data.isBoolean)
            {
                data.isChecked = true;
            }
            else
            {
                data.currentAmount++;
                if (data.currentAmount == data.requirement)
                {
                    data.isChecked = true;
                }
            }
        }
        else
        {
            print("preqrequisites not complete to update this quest");
        }
    }

    static private bool CheckPrerequisiteQuests(FlagData data)
    {
        bool returnValue = true;
        foreach(string flagName in data.prerequisiteObjectives)
        {
            if(questFlags[flagName] != null)
            {
                if (!questFlags[flagName].isChecked)
                {
                    returnValue = false;
                }
            }
            else
            {
                print("the prerequisite ''" + flagName + "'' was not found in questFlags");
            }
        }
        return returnValue;
    }
}