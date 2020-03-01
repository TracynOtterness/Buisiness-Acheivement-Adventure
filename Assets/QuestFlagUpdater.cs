using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestFlagUpdater : MonoBehaviour {
    [SerializeField] string FlagName;
    [SerializeField] string triggerType;

    bool inRange;

	// Use this for initialization
	void Start () {
        if(triggerType == "Start")
        {
            QuestManager.UpdateQuestFlag(FlagName);
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        inRange = true;
        if(triggerType == "Collision")
        {
            QuestManager.UpdateQuestFlag(FlagName);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        inRange = false;
    }

    private void Update()
    {
        if(triggerType != "Interact") { return; }
        if (Input.GetKeyDown(KeyCode.F) && inRange)
        {
            QuestManager.UpdateQuestFlag(FlagName);
        }
    }
}
