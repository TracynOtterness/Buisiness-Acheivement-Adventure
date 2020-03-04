using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnTouch : MonoBehaviour {
    [SerializeField] AudioClip collectionSound;
    [SerializeField] GameObject COSAPrefab;
    [Range(0f, 1f)]
    [SerializeField] float volume;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        CustomOneShotAudio cosa = Instantiate(COSAPrefab, Camera.main.transform).GetComponent<CustomOneShotAudio>();
        cosa.PlayAudio(collectionSound, volume);
        Destroy(gameObject);
    }
}
