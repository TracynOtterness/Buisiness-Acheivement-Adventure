using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempMusic : MonoBehaviour {

    static TempMusic music;

    AudioSource audioSource;
    private void Awake()
    {
        if (music == null)
        {
            music = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
