using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour {

    InteractibilityIcon interactibilityIcon;


    [SerializeField] public Dialogue dialogue;
    public bool inPlayerRange;

    private void Start()
    {
        interactibilityIcon = FindObjectOfType<InteractibilityIcon>();
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
    }
}
