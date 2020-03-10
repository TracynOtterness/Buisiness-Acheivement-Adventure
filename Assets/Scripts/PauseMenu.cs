using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

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

    [Header("Quest requirements")]
    [SerializeField] GameObject scrollViewContent;
    [SerializeField] GameObject questDisplayPrefab;

    [SerializeField] Text nameText;
    [SerializeField] Text descriptionText;
    [SerializeField] Slider progressBar;
    [SerializeField] Text questSliderNumberDisplay;
    [SerializeField] Animator questDisplayAnimator;

    [Header("Minimap Requirements")]
    [SerializeField] GameObject zoomedOutMinimap;
    [SerializeField] GameObject zoomedInMinimap;

    [SerializeField] Image screenshotImage;
    [SerializeField] GameObject FastTravelUIPrefab;
    [SerializeField] GameObject fastTravelScrollViewContent;

    [Header("Other References")]
    [SerializeField] Text livesText;
    [SerializeField] Text coinsText;
    [SerializeField] PopulateQuestGrid ftlGridPopulator;
    [SerializeField] Scrollbar ftlScrollBar;
    [SerializeField] Scrollbar questsScrollBar;

    static int[] KBLevelRequirements = { 0, 4, 8, 12, 15 };
    static float[] explorationLevelRequirements = { 0f, .25f, .5f, .75f, 1f };
    static int[] sideQuestLevelRequirements = {0, 2, 4, 6, 8 };

    public static KBUI[] kBUIs;
    static int kBUIsNumber = -1;
    static int currentKBLevel;
    static int currentExplorationLevel;
    static int currentSideQuestLevel;

    static int currentKBCount;
    static float currentExploration;
    static int currentSideQuestCount;

    static int currentCheckpoints;
    static int totalCheckpoints;

    static bool minimapIsZoomedIn;
    static FastTravelLocation selectedFastTravelLocation;
    public static int ZoomedOutMapXCoord;
    public static int ZoomedOutMapYCoord;

    public static PauseMenu pauseMenu;
    public static int objectivesTotal = 3;
    public static bool canAdvanceToNextLevel;

    private void Awake()
    {
        if (pauseMenu == null)
        {
            pauseMenu = this;
            DontDestroyOnLoad(gameObject.transform.parent.gameObject);
        }
        else
        {
            Destroy(gameObject.transform.parent.gameObject);
        }
    }

    void Start ()
    {
        OrderKBs();
        totalCheckpoints = GameSession.TotalCheckpointsInDifferentLevels[GameSession.currentLevel];
	}

    public void OpenPauseMenu(int lives, int coins)
    {
        DisplayKBProgress();
        DisplayExplorationProgress();
        DisplaySideQuestProgress();
        DisplayLivesAndCoins(lives, coins);
        PopulateQuestGrid();
        ZoomedOutMapXCoord = FindObjectOfType<SceneDataHolder>().data.xCoordinate;
        ZoomedOutMapYCoord = FindObjectOfType<SceneDataHolder>().data.yCoordinate;
        PopulateFastTravelLocations();
        ftlGridPopulator.PopulateGrid();
        ResetScrollViews();
    }

    private void PopulateQuestGrid()
    {
        for (int i = QuestManager.activeQuests.Count - 1; i >= 0; i--) //move completed quests to the end of the list so they are added last
        {
            Quest questToRelocate = QuestManager.activeQuests[i];
            if (questToRelocate.complete)
            {
                QuestManager.activeQuests.Remove(questToRelocate);
                QuestManager.activeQuests.Add(questToRelocate);
            }
        }
        for (int i = 0; i < QuestManager.activeQuests.Count; i++)
        {
            print(QuestManager.activeQuests[i]);
            // Create new instances of our prefab until we've created as many as we specified
            GameObject newObj = Instantiate(questDisplayPrefab, scrollViewContent.transform);

            QuestDisplay qd = newObj.GetComponent<QuestDisplay>();
            qd.quest = QuestManager.activeQuests[i];
            qd.objectiveDisplaySlider = progressBar;
            qd.questDescription = descriptionText;
            qd.questName = nameText;
            qd.questNumberDisplay = questSliderNumberDisplay;
            qd.questDisplayAnimator = questDisplayAnimator;
        }
    }

    public void HideQuestInfo()
    {
        questDisplayAnimator.SetBool("Display", false);
    }

    void OrderKBs()
    {
        kBUIs = FindObjectsOfType<KBUI>();
        KBUI temp;

        for(int i = 0; i < kBUIs.Length - 1; i++)
        {
            for (int j = i+1; j < kBUIs.Length; j++)
            {
                if (kBUIs[i].order > kBUIs[j].order)
                {
                    temp = kBUIs[i];
                    kBUIs[i] = kBUIs[j];
                    kBUIs[j] = temp;
                }
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
            CheckIfNextLevelAvailable();
        }
    }
    public static void IncreaseCheckpointCount()
    {
        currentCheckpoints++;
        print(currentExplorationLevel);
        print(totalCheckpoints);
        if ((float)currentCheckpoints/totalCheckpoints >= explorationLevelRequirements[currentExplorationLevel + 1])
        {
            currentExplorationLevel++;
            CheckIfNextLevelAvailable();
        }
    }
    public static void IncreaseSideQuestCount()
    {
        currentSideQuestCount++;
        if (currentSideQuestCount == sideQuestLevelRequirements[currentSideQuestLevel + 1])
        {
            currentSideQuestLevel++;
            CheckIfNextLevelAvailable();
        }
    }
    public void ResetSideQuestDisplay()
    {
        QuestDisplay[] questDisplays = FindObjectsOfType<QuestDisplay>();
        for (int i = questDisplays.Length; i > 0; i--)
        {
            Destroy(questDisplays[i-1].gameObject);
        }
    }

    public static KBUI GetNextKBUI()
    {
        kBUIsNumber++;
        return kBUIs[kBUIsNumber];
    }

    public void ToggleMinimapZoom()
    {
        print("ToggleMinimapZoom");
        if (minimapIsZoomedIn)
        {
            zoomedInMinimap.GetComponent<CanvasGroup>().alpha = 0;
            zoomedInMinimap.GetComponent<CanvasGroup>().interactable = false;
            zoomedInMinimap.GetComponent<CanvasGroup>().blocksRaycasts = false;

            zoomedOutMinimap.GetComponent<CanvasGroup>().alpha = 1;
            zoomedOutMinimap.GetComponent<CanvasGroup>().interactable = true;
            zoomedOutMinimap.GetComponent<CanvasGroup>().blocksRaycasts = true;

            minimapIsZoomedIn = false;
        }
        else
        {
            zoomedInMinimap.GetComponent<CanvasGroup>().alpha = 1;
            zoomedInMinimap.GetComponent<CanvasGroup>().interactable = true;
            zoomedInMinimap.GetComponent<CanvasGroup>().blocksRaycasts = true;

            zoomedOutMinimap.GetComponent<CanvasGroup>().alpha = 0;
            zoomedOutMinimap.GetComponent<CanvasGroup>().interactable = false;
            zoomedOutMinimap.GetComponent<CanvasGroup>().blocksRaycasts = false;

            minimapIsZoomedIn = true;
        }
    }

    public void SetFastTravelDestination(FastTravelLocation location)
    {
        selectedFastTravelLocation = location;
        screenshotImage.sprite = location.screenshot;
    }

    public void PopulateFastTravelLocations()
    {
        List<FastTravelLocation> allFastTravelLocations = new List<FastTravelLocation>();

        FastTravelUIButton[] oldButtons = FindObjectsOfType<FastTravelUIButton>();
        for (int i = oldButtons.Length - 1; i >= 0; i--) //destroy old buttons
        {
            Destroy(oldButtons[i].gameObject);
        }
        foreach(SceneData s in MasterSceneData.allVisitedScenes) //get all FastTravelLocations
        {
            if(s.xCoordinate == ZoomedOutMapXCoord && s.yCoordinate == ZoomedOutMapYCoord)
            {
                foreach (FastTravelLocation f in s.fastTravelLocations)
                {
                    allFastTravelLocations.Add(f);
                }
            }
        }

        bool defaultFTLisSet = false;
        if(allFastTravelLocations.Count != 0)
        {
            foreach (FastTravelLocation f in allFastTravelLocations)
            {
                if (f.visited)
                {
                    if (!defaultFTLisSet)
                    {
                        GameSession.SetFTL(f);
                        defaultFTLisSet = true;
                        screenshotImage.sprite = f.screenshot;
                    }
                    GameObject newObj = Instantiate(FastTravelUIPrefab, fastTravelScrollViewContent.transform);
                    newObj.GetComponent<FastTravelUIButton>().location = f;
                    newObj.GetComponent<FastTravelUIButton>().SetName();
                }
            }
        }
        else
        {
            print("no fast travel locations have been discovered in this scene yet!");
        }
    }

    public void PurgeCollectedKnowledgeBytes()
    {
        KnowledgeByte[] kbs = FindObjectsOfType<KnowledgeByte>();
        if (kBUIs != null)
        {
            foreach (KBUI kbui in kBUIs)
            {
                for (int i = kbs.Length - 1; i >= 0; i--)
                {
                    bool match = CompareDialogue(kbui.trivia, kbs[i].trivia);
                    if (match)
                    {
                        Destroy(kbs[i].gameObject);
                    }
                }
            }
        }
    }

    bool CompareDialogue(Dialogue d1, Dialogue d2)
    {
        if (!(d1.name == d2.name))
        {
            return false;
        }
        if (!(d1.avatar == d2.avatar))
        {
            return false;
        }
        if(!d1.sentences.SequenceEqual(d2.sentences))
        {
            return false;
        }
        return true;
    }

    static void CheckIfNextLevelAvailable()
    {
        objectivesTotal++;
        print("Knowledge bytes: " + currentKBCount);
        print("KB level: " + currentKBLevel);
        print("Side Quests: " + currentSideQuestCount);
        print("SQ level: " + currentSideQuestLevel);
        print("Checkpoints passed: " + currentCheckpoints);
        print("current exploration level: " + currentExplorationLevel);
        print("Objectives total: " + objectivesTotal);

        if (objectivesTotal >= 10 && currentKBLevel >= 1 && currentSideQuestLevel >=1 && currentExplorationLevel >=1)
        {
            canAdvanceToNextLevel = true;
            Portal[] portals = FindObjectsOfType<Portal>();
            foreach(Portal p in portals)
            {
                if (p.NextLevelPortal)
                {
                    p.OpenPortalIfObjectivesComplete();
                }
            }
        }
    }

    public static void ResetObjectiveProgress()
    {
        ScoreKeeper.knowledgeBytesCollected += currentKBCount;
        ScoreKeeper.sideQuestsCompleted += currentSideQuestCount;
        ScoreKeeper.checkpointsPassed += currentCheckpoints;
        currentKBCount = 0;
        currentCheckpoints = 0;
        currentSideQuestCount = 0;
        currentKBLevel = 0;
        currentSideQuestLevel = 0;
        currentExplorationLevel = 0;
        kBUIsNumber = -1;
        objectivesTotal = 3;
        canAdvanceToNextLevel = false;
    }
    private void ResetScrollViews()
    {
        ftlScrollBar.value = 1f;
        questsScrollBar.value = 1f;
    }

}