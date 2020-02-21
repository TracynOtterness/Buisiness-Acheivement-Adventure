using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour {

    static Dictionary<string, FlagData> questFlags = new Dictionary<string, FlagData>(); //quest flags that exist within the scene
    public static List<Quest> activeQuests = new List<Quest>(); // active quests
    List<Quest> allQuests;

    [SerializeField] GameObject questHolder;
    public static QuestManager questManager;

    private void Awake()
    {
        if (questManager == null)
        {
            questManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        allQuests = new List<Quest>();
        PopulateQuestFlags();
    }


    void StoreQuests(List<Quest> quests)
    {
        for(int i = quests.Count; i > 0; i--)
        {
            GameObject newQuestHolder = Instantiate(questHolder, this.transform);
            Quest questToAdd = newQuestHolder.GetComponent<Quest>();
            allQuests.Add(questToAdd);
            questToAdd.questRequirementNames = quests[i-1].questRequirementNames;
            questToAdd.questRequirementData = quests[i-1].questRequirementData;
            questToAdd.questInfo = quests[i-1].questInfo;
            quests[i-1].GetComponent<NPC>().SetQuest(questToAdd);
            Destroy(quests[i-1]);
        }
    }

    //populateQuestFlags on scene loaded

    public void PopulateQuestFlags()
    {
        Quest[] newQuests = FindObjectsOfType<Quest>();
        List<Quest> newQuestsList = ArrayToList(newQuests);
        print(newQuestsList.Count);
        for (int i = newQuestsList.Count - 1; i >= 0; i--)
        {
            bool isNewQuest = CheckIfNewQuest(newQuestsList[i]);
            if (!isNewQuest)
            {
                newQuestsList.Remove(newQuestsList[i]);
                continue;
            }
            foreach (KeyValuePair<string, FlagData> flag in newQuestsList[i].questRequirements)
            {
                questFlags.Add(flag.Key, flag.Value);
            }
        }
        print(newQuestsList.Count);
        StoreQuests(newQuestsList);
    }

    private List<Quest> ArrayToList(Quest[] newQuests)
    {
        List<Quest> returnList = new List<Quest>();
        foreach(Quest q in newQuests)
        {
            returnList.Add(q);
        }
        return returnList;
    }

    private bool CheckIfNewQuest(Quest q)
    {
        if (allQuests == null) { return true; }
        print("CheckIfNewQuest on quest: " + q.questInfo.name);
        if (q.transform.parent = transform)
        {
            print("old quest");
            return false;
        }
        foreach (Quest existingQuest in allQuests)
        {
            print("existing quest: " + existingQuest.questInfo.name);
            if (q.questInfo.name == existingQuest.questInfo.name)
            {
                print("duplicate");
                return false;
            }
        }
        print("new quest");
        return true;
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
            print(activeQuests[i]);
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