using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

    [SerializeField] AudioClip coinSound;
    [Range(0f, 1f)]
    [SerializeField] float volume;
    [SerializeField] GameObject cosaPrefab;

    bool collected;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collected) { return; }
        collected = true;
        CustomOneShotAudio cosa = Instantiate(cosaPrefab, Camera.main.transform).GetComponent<CustomOneShotAudio>();
        cosa.PlayAudio(coinSound, volume);
        //AudioSource.PlayClipAtPoint(coinSound, Camera.main.transform.position, volume);
        FindObjectOfType<GameSession>().CollectCoin();
        Destroy(gameObject);
    }
}
