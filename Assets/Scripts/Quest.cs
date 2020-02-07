using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest : MonoBehaviour {

    [SerializeField] string[] questRequirementNames;
    [SerializeField] FlagData[] questRequirementData;

    public Dialogue questInfo; //info about the quest for the pause menu
    public Dictionary<string, FlagData> questRequirements = new Dictionary<string, FlagData>(); //list of flags needed to fulfill the quest, in order
    public KeyValuePair<string, FlagData> currentObjective;
    public bool complete;

    public int totalObjectives;
    public int completedObjectives;

    private void Awake()
    {
        totalObjectives = questRequirementNames.Length;
        ConstructQuestRequirements();
    }

    private void ConstructQuestRequirements()
    {
        for (int i = 0; i < questRequirementNames.Length; i++)
        {
            questRequirements.Add(questRequirementNames[i], questRequirementData[i]);
        }
    }

    public void CheckIfComplete()
    {
        completedObjectives = 0;
        foreach(KeyValuePair<string, FlagData> objective in questRequirements)
        {
            if (objective.Value.isChecked)
            {
                completedObjectives++;
            }
        }
        if(completedObjectives == totalObjectives)
        {
            QuestComplete();
        }
    }

    private void QuestComplete()
    {
        PauseMenu.IncreaseSideQuestCount();
        print("Quest: '" + questInfo.name + "' is complete!");
        //QuestManager.RemoveQuest(this);
        complete = true;
    }
}



[Serializable]
public class FlagData
{
    public bool isBoolean;
    public int orderInQuest;
    public int currentAmount;
    public int requirement;
    public bool isChecked;
    public string[] prerequisiteObjectives;
}