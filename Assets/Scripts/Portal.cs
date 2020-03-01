﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour {

    Animator animator;
    bool canWarp;
    public FastTravelLocation location;
    [SerializeField] string locationName;
    [SerializeField] Gate gate;
    [SerializeField] public bool NextLevelPortal;
    public bool objectivesComplete;

    private void Awake()
    {
        location.position = gameObject.transform.position;
        location.nativeScene = FindObjectOfType<SceneDataHolder>().data;
    }

    private void Start()
    {
        animator = GameObject.Find("Warp Icon").GetComponent<Animator>();
        location.position = this.transform.position;
        location.nativeScene = FindObjectOfType<SceneDataHolder>().data;
        location.locationName = this.locationName;
        if (NextLevelPortal && PauseMenu.canAdvanceToNextLevel)
        {
            objectivesComplete = true;
            GetComponent<Animator>().SetTrigger("activated");
        }
        if (!FastTravelReset.ftr.fastTravelLocations.Contains(location))
        {
            Debug.LogError("FastTravelLocation " + location.name + "Is not in FastTravelReset");
        }

        if (location == null)
        {
            Debug.LogError(this + " has no assigned FastTravelLocation");
        }
        if(locationName == null)
        {
            Debug.LogError(this + " has no assigned locationName");
        }
        if(gate == null)
        {
            Debug.LogError(this + " has no assigned gate");
        }
        if(gate.scenes[0] == null || gate.scenes[1] == null || gate.ftls[0] == null || gate.ftls[1] == null)
        {
            Debug.LogError(gate.name + " has missing values");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(NextLevelPortal && !objectivesComplete) { return; }
        canWarp = true;
        animator.SetBool("Hiding", false);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (NextLevelPortal && !objectivesComplete) { return; }
        canWarp = false;
        animator.SetBool("Hiding", true);
    }

    private void Update()
    {
        if (NextLevelPortal)
        {
            if(canWarp && objectivesComplete)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    animator.SetBool("Hiding", true);
                    location.visited = true;
                    print(location + "  " + gate);
                    GameSession.SetFTLByPortal(location, gate);
                    GameSession.gameSession.StopCoroutines();
                    GameSession.gameSession.StartCoroutine(GameSession.gameSession.FadeOut(4));
                }

            }
        }
        else
        {
            if (canWarp)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    animator.SetBool("Hiding", true);
                    location.visited = true;
                    print(location + "  " + gate);
                    GameSession.SetFTLByPortal(location, gate);
                    GameSession.gameSession.StopCoroutines();
                    GameSession.gameSession.StartCoroutine(GameSession.gameSession.FadeOut(3));
                }
            }
        }
    }
}
