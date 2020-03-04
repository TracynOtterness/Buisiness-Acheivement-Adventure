using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class tempsceneswitch : MonoBehaviour {
    Animator animator;
    private void Start()
    {
        animator = GameObject.Find("PlayerSprite").GetComponent<Animator>();
    }
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void StartGame()
    {
        animator.SetTrigger("Start");
        StartCoroutine(LoadStartScene());
    }
    IEnumerator LoadStartScene()
    {
        yield return new WaitForSecondsRealtime(2f);
        SceneManager.LoadScene("Future(0,0)");
    }
}
