using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionManager : MonoBehaviour {

    float lastWidth;
    float lastHeight;
    // Use this for initialization

    public static ResolutionManager resolutionManager;

    private void Awake()
    {
        if (resolutionManager == null)
        {
            resolutionManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    void Start () {
        Screen.SetResolution(1920, 1080, true);
	}
	
	// Update is called once per frame
	void Update () {
        if (System.Math.Abs(lastWidth - Screen.width) > Mathf.Epsilon)
        {
            Screen.SetResolution(Screen.width, Screen.width * (16 / 9), true);
        }
        else if (System.Math.Abs(lastHeight - Screen.height) > Mathf.Epsilon)
        {
            Screen.SetResolution(Screen.height * (9 / 16), Screen.height, true);
        }

        lastWidth = Screen.width;
        lastHeight = Screen.height;
    }
}
