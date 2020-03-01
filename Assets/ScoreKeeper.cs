using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour {

    public static int coinsCollected;
    public static int knowledgeBytesCollected;
    public static int checkpointsPassed;
    public static int sideQuestsCompleted;
    public static int[] completionTimes = new int[4];

    public static bool gamePlay = true;
    public static int currentLevel = 0;


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

    // Use this for initialization
    void Start () {
        StartCoroutine(Timer());
	}


    IEnumerator Timer()
    {
        while (gamePlay)
        {
            yield return new WaitForSeconds(1f);
            completionTimes[currentLevel]++;
        }
    }

    public static void ShowScoreoard()
    {

    }
}
