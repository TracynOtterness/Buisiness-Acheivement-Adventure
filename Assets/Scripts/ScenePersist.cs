using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePersist : MonoBehaviour {

    int sceneIndex = 0;

    private void Awake()
    {
        int scenePersistsInScene = FindObjectsOfType<ScenePersist>().Length;
        if(scenePersistsInScene > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            sceneIndex = SceneManager.GetActiveScene().buildIndex;
        }
    }

    public void DoScenePersist(int buildIndex)
    {
        if(buildIndex != sceneIndex)
        {
            Destroy(gameObject);
        }
    }
}
