﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour {

    InteractibilityIcon interactibilityIcon;
    public Quest quest;


    [SerializeField] public Dialogue dialogue;
    public bool inPlayerRange;

    private void Start()
    {
        interactibilityIcon = FindObjectOfType<InteractibilityIcon>();
        quest = GetComponent<Quest>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        interactibilityIcon.OpenInteractivityPrompt(this);
        inPlayerRange = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        interactibilityIcon.CloseInteractivityPrompt();
        inPlayerRange = false;
        StartCoroutine(CoolDown());
    }

    private IEnumerator CoolDown()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(.5f);
        GetComponent<BoxCollider2D>().enabled = true;
    }

}
