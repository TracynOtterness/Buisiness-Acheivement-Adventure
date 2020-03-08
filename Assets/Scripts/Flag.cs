using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour {

    Animator animator;
    bool passed;
    [SerializeField] GameObject particleEffect;
    [SerializeField] AudioClip soundEffect;
    [SerializeField] public FastTravelLocation location;
    [SerializeField] string locationName;
    [Range(0f, 1f)]
    [SerializeField] float volume;
    [SerializeField] GameObject cosaPrefab;

    private void Awake()
    {
        location.position = gameObject.transform.position;
        location.nativeScene = FindObjectOfType<SceneDataHolder>().data;
        location.locationName = locationName;
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if(location == null)
		{
			Debug.LogError(this.name + " has no attached FastTravelLocation!");
		}
        if(locationName == null)
		{
			Debug.LogError(this.locationName + " has no locationName!");
		}
        if (location.visited) //show checkpoint as passed if warping back
        {
            passed = true;
            animator.SetBool("passed", true);
        }
        print(FastTravelReset.ftr);
        if (!FastTravelReset.ftr.fastTravelLocations.Contains(location))
        {
            Debug.LogError("FastTravelLocation " + location.name + "Is not in FastTravelReset");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (passed)
        {
            if(collision.gameObject.GetComponent<Player>() != null)
            {
                collision.gameObject.GetComponent<Player>().SetCheckpoint(transform.position);
            }
            return;
        }
        if (collision.gameObject.GetComponent<Player>() == null) { return; }
        passed = true;
        location.visited = true;
        animator.SetBool("passed", true);
        PauseMenu.IncreaseCheckpointCount();
        collision.gameObject.GetComponent<Player>().SetCheckpoint(transform.position);
        GameObject particle = Instantiate(particleEffect, this.transform.position, Quaternion.identity);
        CustomOneShotAudio cosa = Instantiate(cosaPrefab, Camera.main.transform).GetComponent<CustomOneShotAudio>();
        cosa.PlayAudio(soundEffect, volume);
        StartCoroutine(CleanUpParticles(particle));
    }

    IEnumerator CleanUpParticles(GameObject p)
    {
        yield return new WaitForSeconds(3f);
        Destroy(p);
    }  
}
