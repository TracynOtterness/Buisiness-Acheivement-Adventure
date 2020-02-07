using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestDisplay : MonoBehaviour {

    public Quest quest;

    public Text questName;
    public Text questDescription;
    public Slider objectiveDisplaySlider;
    public Text questNumberDisplay;
    public Button detailsButton;
    public Animator questDisplayAnimator;

    Image avatar;
    Text nonExapandedQuestName;
    Text detailsButtonText;


    private void Start()
    {
        if (quest == null) { return; } //unneeded in real situation
        CreateQuestUI();
    }

    private void CreateQuestUI()
    {
        avatar = transform.Find("Image").GetComponent<Image>();
        nonExapandedQuestName = transform.Find("Text").GetComponent<Text>();
        nonExapandedQuestName.text = quest.questInfo.name;
        avatar.sprite = quest.questInfo.avatar;
        if (quest.complete)
        {
            detailsButtonText = detailsButton.GetComponentInChildren<Text>();
            detailsButton.interactable = false;
            detailsButtonText.text = "Complete!";
        }
    }

    public void SetupDisplay()
    {
        questName.text = quest.questInfo.name;
        questDescription.text = quest.questInfo.sentences[quest.completedObjectives];
        questNumberDisplay.text = quest.completedObjectives + " / " + quest.totalObjectives;
        objectiveDisplaySlider.value = (float)quest.completedObjectives / quest.totalObjectives;
        questDisplayAnimator.SetBool("Display", true);
    }
}
