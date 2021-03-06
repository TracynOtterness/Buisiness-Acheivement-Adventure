﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour {

    static Dictionary<string, FlagData> questFlags = new Dictionary<string, FlagData>(); //quest flags that exist within the scene
    public static List<Quest> activeQuests = new List<Quest>(); // active quests
    List<Quest> allQuests;

    [SerializeField] GameObject questHolder;
    public static QuestManager questManager;

    [SerializeField] GameObject questCompleteIndicator;
    [SerializeField] Text questCompleteText;

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            foreach(KeyValuePair<string, FlagData> x in questFlags)
            {
                print(x.Key);
            }
        }
    }
    private void Start()
    {
        allQuests = new List<Quest>();
        StartCoroutine(WaitBeforePopulation());
    }

    public IEnumerator WaitBeforePopulation()
    {
        yield return new WaitForSeconds(.1f);
        PopulateQuestFlags();
    }

    void StoreQuests(List<Quest> newQuests, List<Quest> duplicateQuests, List<Quest> existingQuests)
    {
        for(int i = newQuests.Count - 1; i >= 0; i--)
        {
            GameObject newQuestHolder = Instantiate(questHolder, this.transform);
            Quest questToAdd = newQuestHolder.GetComponent<Quest>();
            allQuests.Add(questToAdd);
            questToAdd.questAcceptFlag = newQuests[i].questAcceptFlag;
            questToAdd.questRequirementNames = newQuests[i].questRequirementNames;
            questToAdd.questRequirementData = newQuests[i].questRequirementData;
            questToAdd.questInfo = newQuests[i].questInfo;
            questToAdd.totalObjectives = newQuests[i].totalObjectives;
            newQuests[i].GetComponent<NPC>().SetQuest(questToAdd);
            Destroy(newQuests[i]);
        }
        for(int i = duplicateQuests.Count - 1; i >= 0; i--)
        {
            duplicateQuests[i].GetComponent<NPC>().SetQuest(existingQuests[i]);
            Destroy(duplicateQuests[i]);
        }
    }

    //populateQuestFlags on scene loaded

    void PopulateQuestFlags()
    {
        Quest[] newQuests = FindObjectsOfType<Quest>();
        List<Quest> newQuestsList = ArrayToList(newQuests); //list of new quests to add
        List<Quest> duplicateQuestsList = new List<Quest>();//list of duplicate quests
        List<Quest> existingQuestsList = new List<Quest>();//list of quests that the duplicate quests are duplicates of
        for (int i = newQuestsList.Count - 1; i >= 0; i--)
        {
            int isNewQuest = CheckIfNewQuest(newQuestsList[i]);
            if (isNewQuest != 2) //it's not a new quest
            {
                if(isNewQuest == 1) //it's a duplicate, and the NPC needs a quest
                {
                    duplicateQuestsList.Add(newQuestsList[i]);
                    existingQuestsList.Add(GetExistingQuest(newQuestsList[i]));
                }
                newQuestsList.Remove(newQuestsList[i]);
                continue;
            }
            foreach (KeyValuePair<string, FlagData> flag in newQuestsList[i].questRequirements)
            {
                questFlags.Add(flag.Key, flag.Value);
            }
        }
        print("Duplicate Quests: " + existingQuestsList.Count);
        StoreQuests(newQuestsList, duplicateQuestsList, existingQuestsList);
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

    private int CheckIfNewQuest(Quest q)
    {
        if (allQuests == null) { return 0; }
        if (q.transform.parent == transform)
        {
            return 0;
        }
        foreach (Quest existingQuest in allQuests)
        {
            if (q.questInfo.name == existingQuest.questInfo.name)
            {
                return 1;
            }
        }
        return 2;
    }

    private Quest GetExistingQuest(Quest quest)
    {
        foreach (Quest existingQuest in allQuests)
        {
            if (quest.questInfo.name == existingQuest.questInfo.name)
            {
                return existingQuest;
            }
        }
        return null;
    }

    public static void AddQuest(Quest questToAdd)
    {
        activeQuests.Add(questToAdd);
        questToAdd.CheckIfComplete();
    }

    public static void UpdateQuestFlag(string flagName)
    {
        if(questFlags.ContainsKey(flagName))
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
                activeQuests[i].CheckIfComplete();
            }
            else
            {
                print("complete");
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

    public static void ResetFlags()
    {
        questFlags.Clear();
    }

    public static void CompleteQuestIndicator(string questName)
    {
        Animator a = questManager.questCompleteIndicator.GetComponent<Animator>();
        a.SetBool("Show", true);
        questManager.StartCoroutine(questManager.TakeBackQuestCompleteIndicator(a));
        questManager.questCompleteText.text = questName;
    }

    public IEnumerator TakeBackQuestCompleteIndicator(Animator a)
    {
        yield return new WaitForSeconds(2f);
        a.SetBool("Show", false);
    }
}