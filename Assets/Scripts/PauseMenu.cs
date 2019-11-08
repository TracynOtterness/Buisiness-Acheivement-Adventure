using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {

    [Header("Objective Progress Display References")]
    [SerializeField] Text KBLevelText;
    [SerializeField] Text KBNumberDisplay;
    [SerializeField] Slider KBSlider;

    [SerializeField] Text explorationLevelText;
    [SerializeField] Text explorationNumberDisplay;
    [SerializeField] Text explorationBoundsDisplay;
    [SerializeField] Slider explorationSlider;

    [SerializeField] Text sideQuestLevelText;
    [SerializeField] Text sideQuestNumberDisplay;
    [SerializeField] Slider sideQuestSlider;

    [SerializeField] Text livesText;
    [SerializeField] Text coinsText;

    static int[] KBLevelRequirements = { 0, 4, 8, 12, 15 };
    static float[] explorationLevelRequirements = { 0f, .25f, .5f, .75f, 1f };
    static int[] sideQuestLevelRequirements = {0, 2, 4, 6, 8 };

    static int currentKBLevel;
    static int currentExplorationLevel;
    static int currentSideQuestLevel;

    static int currentKBCount;
    static float currentExploration;
    static int currentSideQuestCount;

    static int currentCheckpoints;
    static int totalCheckpoints;

	void Start ()
    {
        AssignKBs();
        totalCheckpoints = FindObjectsOfType<Flag>().Length;
	}

    public void OpenPauseMenu(int lives, int coins)
    {
        DisplayKBProgress();
        DisplayExplorationProgress();
        DisplaySideQuestProgress();
        DisplayLivesAndCoins(lives, coins);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void AssignKBs()
    {
        KBUI[] kbuis = FindObjectsOfType<KBUI>();
        KnowledgeByte[] kbs = FindObjectsOfType<KnowledgeByte>();
        if (kbuis.Length == kbs.Length)
        {
            for (int i = 0; i < kbuis.Length; i++)
            {
                kbs[i].kbui = kbuis[i];
                kbuis[i].trivia = kbs[i].trivia;
            }
        }
        else
        {
            if (kbuis.Length > kbs.Length)
            {
                print("There are not enough KnowledgeBytes in the scene!");
            }
            else
            {
                print("There are not enough KnowledgeBytes in the scene!");
            }
        }
    }

    private void DisplayExplorationProgress()
    {
        explorationLevelText.text = (currentExplorationLevel + 1).ToString();
        if (currentExplorationLevel == 4)
        {
            explorationBoundsDisplay.text = "";
            explorationNumberDisplay.text = "100%";
            explorationSlider.value = 1f;
        }
        else
        {
            int currentExplorationPercent = Mathf.RoundToInt(currentCheckpoints * 100f / totalCheckpoints);
            explorationNumberDisplay.text = currentExplorationPercent.ToString() + "%";
            string currentExplorationBounds = (explorationLevelRequirements[currentExplorationLevel] * 100f).ToString() + "%                 " + (explorationLevelRequirements[currentExplorationLevel + 1] * 100f).ToString() + "%";
            explorationBoundsDisplay.text = currentExplorationBounds;
            print((currentExplorationPercent / 100 - explorationLevelRequirements[currentExplorationLevel]) / (explorationLevelRequirements[currentExplorationLevel + 1] - explorationLevelRequirements[currentExplorationLevel]));
            explorationSlider.value = (currentExplorationPercent / 100f - explorationLevelRequirements[currentExplorationLevel]) / (explorationLevelRequirements[currentExplorationLevel + 1] - explorationLevelRequirements[currentExplorationLevel]);
        }
    }

    private void DisplayKBProgress()
    {
        KBLevelText.text = (currentKBLevel + 1).ToString();
        if (currentKBLevel == 4)
        {
            KBNumberDisplay.text = "100%";
            KBSlider.value = 1f;
        }
        else
        {
            int currentLevelProgressCount = currentKBCount - KBLevelRequirements[currentKBLevel];
            int currentLevelGoal = KBLevelRequirements[currentKBLevel + 1] - KBLevelRequirements[currentKBLevel];
            KBNumberDisplay.text = currentLevelProgressCount.ToString() + " / " + currentLevelGoal.ToString();
            KBSlider.value = (float)currentLevelProgressCount / currentLevelGoal;
        }
    }

    private void DisplaySideQuestProgress()
    {
        sideQuestLevelText.text = (currentSideQuestLevel + 1).ToString();
        if (currentSideQuestLevel == 4)
        {
            sideQuestNumberDisplay.text = "100%";
            sideQuestSlider.value = 1f;
        }
        else
        {
            int currentLevelProgressCount = currentSideQuestCount - sideQuestLevelRequirements[currentSideQuestLevel];
            int currentLevelGoal = sideQuestLevelRequirements[currentSideQuestLevel + 1] - sideQuestLevelRequirements[currentSideQuestLevel];
            sideQuestNumberDisplay.text = currentLevelProgressCount.ToString() + " / " + currentLevelGoal.ToString();
            sideQuestSlider.value = (float)currentLevelProgressCount / currentLevelGoal;
        }
    }

    void DisplayLivesAndCoins(int lives, int coins)
    {
        livesText.text = "x" + lives.ToString();
        coinsText.text = "x" + coins.ToString();
    }

    public static void IncreaseKBCount()
    {
        currentKBCount++;
        if(currentKBCount == KBLevelRequirements[currentKBLevel + 1])
        {
            currentKBLevel++;
        }
    }
    public static void IncreaseCheckpointCount()
    {
        currentCheckpoints++;
        if ((float)currentCheckpoints/totalCheckpoints >= explorationLevelRequirements[currentExplorationLevel + 1])
        {
            currentExplorationLevel++;
        }
    }
    public static void IncreaseSideQuestCount()
    {
        currentSideQuestCount++;
        if (currentSideQuestCount == sideQuestLevelRequirements[currentSideQuestLevel + 1])
        {
            currentSideQuestLevel++;
        }
    }
}
