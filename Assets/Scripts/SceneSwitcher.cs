using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour {

    [SerializeField] Animator animator;
    Animator myAnimator;

    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
    }

    public void Load(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void StartGame()
    {
        animator.SetTrigger("Start");
        StartCoroutine(LoadStartScene());
    }

    IEnumerator LoadStartScene()
    {
        yield return new WaitForSecondsRealtime(2f);
        Load("Learn_Movement");
    }
}
