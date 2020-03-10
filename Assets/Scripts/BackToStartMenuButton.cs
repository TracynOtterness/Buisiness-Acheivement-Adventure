using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToStartMenuButton : MonoBehaviour {

	public void DoIt()
    {
        try
        {
            ScoreKeeper sc = FindObjectOfType<ScoreKeeper>();
            Destroy(sc.gameObject);
            Music m = FindObjectOfType<Music>();
            Destroy(m.gameObject);
            GameSession gs = FindObjectOfType<GameSession>();
            gs.ClearDontDestroyOnLoadForNextLevel();
            Destroy(gs.gameObject);
            SceneSwitcher ss = FindObjectOfType<SceneSwitcher>();
            Destroy(ss.gameObject);
        }
        catch
        {

        }
        SceneManager.LoadScene("Start_Menu");
    }

    public void Quit()
    {
        print("Quit App");
        Application.Quit();
    }
}
