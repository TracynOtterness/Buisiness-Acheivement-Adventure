using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToStartMenuButton : MonoBehaviour {

	public void DoIt()
    {
        SceneManager.LoadScene("Start_Menu");
    }
}
