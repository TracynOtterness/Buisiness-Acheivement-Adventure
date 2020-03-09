using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour {

    [SerializeField] AudioClip[] audioClips;
    static AudioSource audioSource;
    static Music music;
    static int lastScene = -1;

    private void Awake()
    {
        if (music == null)
        {
            music = this;
            DontDestroyOnLoad(this.gameObject);
            audioSource = GetComponent<AudioSource>();
            lastScene = FindObjectOfType<SceneDataHolder>().data.musicType;
            ChangeSong(FindObjectOfType<SceneDataHolder>().data, true);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public static void RemoveTempPlayer()
    {
        try
        {
            GameObject objectToDestroy = FindObjectOfType<TempMusic>().gameObject;
            if (objectToDestroy != null)
            {
                Destroy(FindObjectOfType<TempMusic>().gameObject);
            }
        }
        catch 
        {
            print("not the first level");
        }
    }

    public static void ChangeSong(SceneData scene, bool awake = false)
    {
        if(lastScene == -1) { return; }
        if(scene.musicType != lastScene)
        {
            audioSource.clip = Music.music.audioClips[scene.musicType];
            audioSource.Play();
            lastScene = scene.musicType;
        }
        else if (awake)
        {
            audioSource.clip = Music.music.audioClips[scene.musicType];
            audioSource.Play();
            lastScene = scene.musicType;
        }
    }

    private void Update()
    {
        this.transform.position = Camera.main.transform.position;
    }
}
