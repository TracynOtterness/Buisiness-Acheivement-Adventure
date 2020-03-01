using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestPrompt : MonoBehaviour {

    [SerializeField] Image profile;
    [SerializeField] Text bodyText;
    [SerializeField] Text questName;

    Animator myAnimator;

    Quest storedQuest;

    private void Start()
    {
        myAnimator = GetComponent<Animator>();
    }

    public void SetupPrompt(Quest questToDisplay)
    {
        storedQuest = questToDisplay;
        myAnimator.SetBool("isDisplaying", true);
        profile.sprite = questToDisplay.questInfo.avatar;
        questName.text = questToDisplay.questInfo.name;
        bodyText.text = questToDisplay.questInfo.sentences[0];
    }

    public void ClosePrompt(bool isAccepted)
    {
        print("close prompt");
        myAnimator.SetBool("isDisplaying", false);
        if (isAccepted)
        {
            if(storedQuest.questAcceptFlag != null)
            {
                print("f");
                QuestManager.UpdateQuestFlag(storedQuest.questAcceptFlag);
            }
            QuestManager.AddQuest(storedQuest);
        }
    }
}
