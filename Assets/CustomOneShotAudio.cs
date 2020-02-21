using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomOneShotAudio : MonoBehaviour {

    AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void PlayAudio(AudioClip audio, float volume)
    {
        audioSource.clip = audio;
        audioSource.volume = volume;
        audioSource.Play();
        StartCoroutine(WaitAndDestroy(audio.length));
    }

    IEnumerator WaitAndDestroy(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
