using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundIndicator : MonoBehaviour {

    [SerializeField] GameObject cosaPrefab;
    [SerializeField] AudioClip sound;
    [Range(0f, 1f)]
    [SerializeField] float volume;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>() != null)
        {
            CustomOneShotAudio cosa = Instantiate(cosaPrefab, Camera.main.transform).GetComponent<CustomOneShotAudio>();
            cosa.PlayAudio(sound, volume);
        }
    }
}
