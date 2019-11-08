using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour {

    Animator animator;
    bool passed;
    [SerializeField] GameObject particleEffect;
    [SerializeField] AudioClip soundEffect;
    AudioSource audioSource;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (passed) { return; }
        if (collision.gameObject.GetComponent<Player>() == null) { return; }
        passed = true;
        animator.SetBool("passed", true);
        PauseMenu.IncreaseCheckpointCount();
        print(collision.gameObject);
        collision.gameObject.GetComponent<Player>().SetCheckpoint(transform.position);
        GameObject particle = Instantiate(particleEffect, this.transform.position, Quaternion.identity);
        Camera.main.gameObject.GetComponent<AudioSource>().PlayOneShot(soundEffect, 1f);
        StartCoroutine(CleanUpParticles(particle));
    }

    IEnumerator CleanUpParticles(GameObject p)
    {
        yield return new WaitForSeconds(3f);
        Destroy(p);
    }  
}
