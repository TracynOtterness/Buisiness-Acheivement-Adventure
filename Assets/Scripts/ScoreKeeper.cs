using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreKeeper : MonoBehaviour {

    public static int coinsCollected;
    public static int knowledgeBytesCollected;
    public static int checkpointsPassed;
    public static int sideQuestsCompleted;

    [SerializeField] GameObject scoreBoard;
    [SerializeField] Text kbText;
    [SerializeField] Text coinsText;
    [SerializeField] Text flagsText;
    [SerializeField] Text questsText;


	public static int currentLevel = 3;
    public static ScoreKeeper scoreKeeper;

    private void Awake()
    {
        if (scoreKeeper == null)
        {
            scoreKeeper = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public static void ShowScoreoard()
    {
        ScoreKeeper.scoreKeeper.ActuallyShowScorecard();
    }

    public void ActuallyShowScorecard()
    {
        scoreBoard.SetActive(true);
        kbText.text = knowledgeBytesCollected.ToString();
        coinsText.text = coinsCollected.ToString();
        flagsText.text = checkpointsPassed.ToString();
        questsText.text = sideQuestsCompleted.ToString();

    }
}
