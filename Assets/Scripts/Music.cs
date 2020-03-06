using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour {

    [SerializeField] AudioClip[] audioClips;
    static AudioSource audioSource;
    static Music music;
    static int lastScene;

    private void Awake()
    {
        if (music == null)
        {
            music = this;
            DontDestroyOnLoad(this.gameObject);
            audioSource = GetComponent<AudioSource>();
            ChangeSong(FindObjectOfType<SceneDataHolder>().data);
            lastScene = FindObjectOfType<SceneDataHolder>().data.buildIndex;
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

    public static void ChangeSong(SceneData scene)
    {
        if(scene.buildIndex != lastScene)
        {
            audioSource.clip = Music.music.audioClips[scene.musicType];
            audioSource.Play();
            lastScene = scene.buildIndex;
        }
    }

    private void Update()
    {
        this.transform.position = Camera.main.transform.position;
    }
}
