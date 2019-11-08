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

    [SerializeField] float deathFadeOutScaleFactor = .1f;

    [SerializeField] GameObject deathMask;
    [SerializeField] Animator UIFaderAnimator;
    [SerializeField] SpriteRenderer deathBlack;
    [SerializeField] Animator gameOverAnimator;
    [SerializeField] Text gameOverLivesText;

    [SerializeField] Animator pauseMenuAnimator;
    [SerializeField] PauseMenu pauseMenu;

    int continues = 0;
    bool pauseMenuIsUp;

    Canvas levelCanvas;
    DialogueManager dialogueManager;
    static GameSession gameSession;

    private void Awake()
    {
        if(gameSession == null)
        {
            gameSession = this;
            DontDestroyOnLoad(gameObject);
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
        dialogueManager = FindObjectOfType<DialogueManager>();
        levelCanvas = FindObjectOfType<Canvas>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePauseMenu();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            PauseMenu.IncreaseSideQuestCount();
        }
    }

    public void CollectCoin()
    {
        coins++;
        coinsText.text = coins.ToString();
    }

    public void CollectKnowledgeByte(Dialogue dialogue)
    {
        knowledgeBytes++;
        dialogueManager.StartDialogue(dialogue);
    }

    public void ProcessPlayerDeath()
    {
        UIFaderAnimator.SetBool("hiding", true);
        StopAllCoroutines();
        playerLives--;
        if (playerLives >= 0)
        {
            StartCoroutine(DeathFadeOut(false));
        }
        else
        {
            StartCoroutine(DeathFadeOut(true));
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

    IEnumerator DeathFadeOut(bool gameOver)
    {
        print("FadeOut");
        deathBlack.enabled = true;
        while (deathMask.transform.localScale.x > 0)
        {
            Vector3 newScale = new Vector3(deathMask.transform.localScale.x - deathFadeOutScaleFactor, deathMask.transform.localScale.y - deathFadeOutScaleFactor, deathMask.transform.localScale.z);
            deathMask.transform.localScale = newScale;
            yield return null;
        }
        deathMask.transform.localScale = Vector3.zero;
        if (gameOver)
        {
            GameOver();
        }
        else
        {
            Respawn();
            print("Respawn from normal death");
        }
    }

    IEnumerator RespawnFadeIn()
    {
        print("FadeIn");
        livesText.text = playerLives.ToString();
        while (deathMask.transform.localScale.x < 2)
        {
            Vector3 newScale = new Vector3(deathMask.transform.localScale.x + deathFadeOutScaleFactor, deathMask.transform.localScale.y + deathFadeOutScaleFactor, deathMask.transform.localScale.z);
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
        print("Respawn from continue");
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
    }

    void Unpause()
    {
        Time.timeScale = 1;
        UIFaderAnimator.SetBool("hiding", false);
        pauseMenuAnimator.SetBool("Pause", false);
        pauseMenuIsUp = false;
    }

}
