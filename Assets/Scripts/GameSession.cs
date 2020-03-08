using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSession : MonoBehaviour {

    [SerializeField] int playerLives = 3;
    [SerializeField] int continuePlayerLives = 3;
    [SerializeField] int coins = 0;
    [SerializeField] int knowledgeBytes;

    [SerializeField] Text livesText;
    [SerializeField] Text coinsText;

    [SerializeField] float fadeOutScaleFactor;

    public GameObject deathMask;
    [SerializeField] Animator UIFaderAnimator;
    public SpriteRenderer deathBlack;
    public Animator gameOverAnimator;
    public Text gameOverLivesText;

    public InteractibilityIcon ii;

    public Animator pauseMenuAnimator;
    public PauseMenu pauseMenu;

    public static int currentLevel = 0;
    static SceneData currentScene;
    static List<SceneData> allVisitedScenes;
    static List<Gate> allVisitedGates;
    static FastTravelLocation fastTravelLocation;

    public static int[] TotalCheckpointsInDifferentLevels = { 6,11,16,11 };

    int continues = 0;
    bool pauseMenuIsUp;

    Canvas levelCanvas;
    DialogueManager dialogueManager;
    public static GameSession gameSession;
    int originalSceneIndex;
    bool isFirstLoad = true;

    private void Awake()
    {
        if(gameSession == null)
        {
            gameSession = this;
            DontDestroyOnLoad(gameObject);
            originalSceneIndex = SceneManager.GetActiveScene().buildIndex;
            currentLevel = ScoreKeeper.currentLevel;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        livesText.text = playerLives.ToString();
        coinsText.text = coins.ToString();
        GetReferences();
        allVisitedGates = new List<Gate>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePauseMenu();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            QuestManager.UpdateQuestFlag("Kill9Bunnies");
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            QuestManager.UpdateQuestFlag("Objective2");
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            QuestManager.UpdateQuestFlag("Objective3");
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            QuestManager.UpdateQuestFlag("PressH10Times");
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            PauseMenu.IncreaseSideQuestCount();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            QuestManager.UpdateQuestFlag("Quest1");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            QuestManager.UpdateQuestFlag("Quest2");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            QuestManager.UpdateQuestFlag("Quest3");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            QuestManager.UpdateQuestFlag("Quest4");
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            QuestManager.UpdateQuestFlag("Quest5");
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            QuestManager.UpdateQuestFlag("Quest6");
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            QuestManager.UpdateQuestFlag("Quest7");
        }
    }

    public void GetReferences()
    {
        currentScene = FindObjectOfType<SceneData>();
        deathBlack = Camera.main.GetComponentInChildren<SpriteRenderer>();
        deathMask = FindObjectOfType<Player>().GetComponentInChildren<SpriteMask>().gameObject;

        if (dialogueManager == null) { dialogueManager = FindObjectOfType<DialogueManager>(); }
        if(levelCanvas == null) { levelCanvas = GameObject.Find("GameLevelCanvas").GetComponent<Canvas>(); }
        if(pauseMenu == null) { pauseMenu = FindObjectOfType<PauseMenu>(); }
        if(pauseMenuAnimator == null) { pauseMenuAnimator = pauseMenu.GetComponent<Animator>(); }
        if(gameOverAnimator == null) { gameOverAnimator = GameObject.Find("GameOverMenu").GetComponent<Animator>(); }
        if(gameOverLivesText == null) { gameOverLivesText = GameObject.Find("GameOverLivesText").GetComponent<Text>(); }
        if(ii == null) { ii = FindObjectOfType<InteractibilityIcon>(); }
    }

    public void CollectCoin()
    {
        coins++;
        coinsText.text = coins.ToString();
    }

    public void CollectKnowledgeByte(Dialogue dialogue)
    {
        knowledgeBytes++;
        FindObjectOfType<InteractibilityIcon>().CloseInteractivityPrompt();
        dialogueManager.StartDialogue(dialogue, null);
    }

    public void ProcessPlayerDeath()
    {
        UIFaderAnimator.SetBool("hiding", true);
        StopAllCoroutines();
        playerLives--;
        if (playerLives >= 0)
        {
            StartCoroutine(FadeOut(2));
        }
        else
        {
            StartCoroutine(FadeOut(1));
        }
    }

    private void GameOver()
    {
        gameOverAnimator.SetTrigger("GameOver");
    }

    public void Continue()
    {
        print("Continue");
        continues++;
        playerLives = continuePlayerLives;
        StartCoroutine(ContinueAnimation());
    }

    public void ReturnToTitleScreen()
    {
        SceneManager.LoadScene(0);
    }

    public static void ChangeLivesCountWrapper()
    {
        gameSession.ChangeLivesCount();
    }

    void ChangeLivesCount()
    {
        playerLives--;
        livesText.text = playerLives.ToString();
    }

    public IEnumerator FadeOut(int scenario)
    {
        FindObjectOfType<Player>().controllable = false;
        deathBlack.enabled = true;
        while (deathMask.transform.localScale.x > 0)
        {
            Vector3 newScale = new Vector3(deathMask.transform.localScale.x - fadeOutScaleFactor * Time.deltaTime, deathMask.transform.localScale.y - fadeOutScaleFactor * Time.deltaTime, deathMask.transform.localScale.z);
            deathMask.transform.localScale = newScale;
            yield return null;
        }
        deathMask.transform.localScale = Vector3.zero;

        switch (scenario)
        {
            case 1: GameOver(); break; //Game Over
            case 2: Respawn(); print("Respawn from normal death"); break; //Normal Death
            case 3: Warp(); break; //Fast Travel
            case 4: if (currentLevel != 3) { EndLevel(); } else { EndGame(); } break; //Go to next level
        }
    }

    IEnumerator RespawnFadeIn()
    {
        FindObjectOfType<Player>().controllable = true;
        livesText.text = playerLives.ToString();
        while (deathMask.transform.localScale.x < 2)
        {
            Vector3 newScale = new Vector3(deathMask.transform.localScale.x + fadeOutScaleFactor * Time.deltaTime, deathMask.transform.localScale.y + fadeOutScaleFactor * Time.deltaTime, deathMask.transform.localScale.z);
            deathMask.transform.localScale = newScale;
            yield return null;
        }
        deathMask.transform.localScale = new Vector3(2,2, deathMask.transform.localScale.z);
        UIFaderAnimator.SetBool("hiding", false);
        deathBlack.enabled = false;
    }

    IEnumerator ContinueAnimation()
    {
        gameOverAnimator.SetTrigger("DoContinueAnimation");
        int livesGiven = 0;
        while(livesGiven < continuePlayerLives)
        {
            livesGiven++;
            gameOverLivesText.text = "x" + livesGiven.ToString();
            yield return new WaitForSecondsRealtime(82f/24f / continuePlayerLives);
        }
        Respawn();
    }

    void Respawn()
    {
        FindObjectOfType<Player>().Respawn();
        StartCoroutine(RespawnFadeIn());
    }




    void TogglePauseMenu()
    {
        if (pauseMenuIsUp)
        {
            Unpause();
        }
        else
        {
            Pause();
        }
    }

    void Pause()
    {
        Time.timeScale = 0;
        UIFaderAnimator.SetBool("hiding", true);
        pauseMenuAnimator.SetBool("Pause", true);
        pauseMenu.OpenPauseMenu(playerLives, coins);
        pauseMenuIsUp = true;
        pauseMenu.GetComponent<CanvasGroup>().interactable = true;
    }

    public void Unpause()
    {
        Time.timeScale = 1;
        UIFaderAnimator.SetBool("hiding", false);
        pauseMenuAnimator.SetBool("Pause", false);
        PauseMenu.pauseMenu.HideQuestInfo();
        pauseMenuIsUp = false;
        pauseMenu.GetComponent<CanvasGroup>().interactable = false;
    }



    public static void SetFTL(FastTravelLocation ftl)
    {
        print(ftl.locationName);
        fastTravelLocation = ftl;
        print(fastTravelLocation.locationName);
    }

    public void HideUI()
    {
        UIFaderAnimator.SetBool("hiding", true);
    }
    void Warp() //Once faded out, swaps over to new scene.
    {
        print(fastTravelLocation.locationName);
        print(fastTravelLocation.nativeScene.xCoordinate + ", " + fastTravelLocation.nativeScene.yCoordinate);
        int sceneToLoad = fastTravelLocation.nativeScene.buildIndex;
        SceneManager.LoadScene(sceneToLoad);
    }

    static void AddGate(Gate g)
    {
        if (!allVisitedGates.Contains(g))
        {
            allVisitedGates.Add(g);
        }
    }

    public static void SetFTLByPortal(FastTravelLocation ftl, Gate g, FastTravelLocation[] ftls)
    {
        print(g);
        AddGate(g);
        print(g.ftls.Length);
        print(ftls.Length);

        for(int i = 0; i < ftls.Length; i++) 
        {
            print("a fastTravelLocation in this gate is: " + ftls[i].locationName);
            if (ftls[i].locationName != ftl.locationName)
            {
                print("it's what we're after");
                print(ftls[i].locationName);
                SetFTL(ftls[i]);
                ftls[i].visited = true;
                break;
            }
        }
    }


    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelLoaded;
    }

    void OnLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        GetReferences();
        StartCoroutine(RespawnFadeIn());

        if(fastTravelLocation != null)
        {
            print(fastTravelLocation.name);
            print("Changing player spawnPosition to " + fastTravelLocation.position);
            FindObjectOfType<Player>().SetupSpawnPosition(fastTravelLocation.position);
        }

        pauseMenu.PurgeCollectedKnowledgeBytes();
        NPC[] npcs = FindObjectsOfType<NPC>();
        foreach(NPC npc in npcs)
        {
            npc.interactibilityIcon = ii;
        }

        if (!isFirstLoad)
        {
            QuestManager.questManager.StartCoroutine(QuestManager.questManager.WaitBeforePopulation()); //wait a second for quests to load before making them permanent
            Music.ChangeSong(FindObjectOfType<SceneDataHolder>().data);
        }
        else
        {
            Music.RemoveTempPlayer();
        }
        isFirstLoad = false;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelLoaded;
    }

    public void StopCoroutines()
    {
        StopAllCoroutines();
    }


    private void EndLevel()
    {
        print("EndLevel");
        print(currentLevel);
        int sceneToLoad = fastTravelLocation.nativeScene.buildIndex;
        ClearDontDestroyOnLoadForNextLevel();
        ScoreKeeper.currentLevel++;
        ScoreKeeper.coinsCollected += coins;
        SceneSwitcher.sceneSwitcher.StartCoroutine(SceneSwitcher.LoadWithDelay(sceneToLoad));
        Destroy(gameObject);
    }

    void ClearDontDestroyOnLoadForNextLevel()
    {
        Destroy(FastTravelReset.ftr.gameObject);
        Destroy(MasterSceneData.masterSceneData.gameObject);
        PauseMenu.ResetObjectiveProgress();
        Destroy(levelCanvas.gameObject);
        QuestManager.activeQuests.Clear();
        Destroy(QuestManager.questManager.gameObject);
    }

    void EndGame()
    {
        print("EndLevel");
        ScoreKeeper.coinsCollected += coins;
        PauseMenu.ResetObjectiveProgress();
        ScoreKeeper.ShowScoreoard();
    }
    public void Quit()
    {
        Application.Quit();
    }

}