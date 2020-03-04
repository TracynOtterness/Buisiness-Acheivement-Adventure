using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour {

    [SerializeField] AudioClip[] audioClips;
    static AudioSource audioSource;
    static Music music;

    private void Awake()
    {
        if (music == null)
        {
            music = this;
            DontDestroyOnLoad(this.gameObject);
            audioSource = GetComponent<AudioSource>();
            ChangeSong(FindObjectOfType<SceneDataHolder>().data.musicType);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public static void ChangeSong(int song)
    {
        audioSource.clip = Music.music.audioClips[song];
        audioSource.Play();
    }

    private void Update()
    {
        this.transform.position = Camera.main.transform.position;
    }
}
