using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

    [SerializeField] AudioClip coinSound;
    //bool hasBeenTriggered = false; unneccesary?

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (hasBeenTriggered) { return; }   unneccesary?
        //hasBeenTriggered = true;   unneccesary?
        AudioSource.PlayClipAtPoint(coinSound, Camera.main.transform.position);
        FindObjectOfType<GameSession>().CollectCoin();
        Destroy(gameObject);
    }
}
