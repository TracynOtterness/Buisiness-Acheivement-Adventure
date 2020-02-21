using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour {

    [SerializeField] Animator animator;
    Animator myAnimator;

    public static SceneSwitcher sceneSwitcher;

    private void Awake()
    {
        if (sceneSwitcher == null)
        {
            sceneSwitcher = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        myAnimator = GetComponent<Animator>();
    }

    public static void Load(string sceneName)
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
        Load("1-(2,2)");
    }
}
