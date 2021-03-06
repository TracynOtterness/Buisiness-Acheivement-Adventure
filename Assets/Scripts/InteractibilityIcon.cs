﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractibilityIcon : MonoBehaviour {

    public Animator animator;
    DialogueManager dialogueManager;
    public bool canInteract;
    public NPC interactableNPC;

    private void Awake()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    public void OpenInteractivityPrompt(NPC newNPC)
    {
        if (dialogueManager.dialoging) { return; }
        print(animator);
        animator.SetBool("ShowPrompt", true);
        canInteract = true;
        interactableNPC = newNPC;
    }

    public void CloseInteractivityPrompt()
    {
        animator.SetBool("ShowPrompt", false);
        canInteract = false;
        interactableNPC = null;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V)) { print("IIs: " + FindObjectsOfType<InteractibilityIcon>().Length); }
        if (dialogueManager.dialoging) { ManualNextSentence(); return; }
        if (!canInteract) { return; }
        if (Input.GetKeyDown(KeyCode.F))
        {
            animator.SetBool("ShowPrompt", false);
        }
    }

    public void StartDialogue()
    {
        if(interactableNPC == null) { return; }
        dialogueManager.StartDialogue(interactableNPC.dialogue, interactableNPC.quest);
        canInteract = false;
        interactableNPC = null;
    }

    private void ManualNextSentence()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            dialogueManager.DisplayNextSentence();
        }
    }
}
