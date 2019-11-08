using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractibilityIcon : MonoBehaviour {

    Animator animator;
    DialogueManager dialogueManager;
    public bool canInteract;
    public NPC interactableNPC;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        dialogueManager = FindObjectOfType<DialogueManager>();
    }
    public void OpenInteractivityPrompt(NPC newNPC)
    {
        if (dialogueManager.dialoging) { return; }
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
        if (dialogueManager.dialoging) { ManualNextSentence(); }
        if (!canInteract) { return; }
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!dialogueManager.dialoging)
            {
                dialogueManager.StartDialogue(interactableNPC.dialogue);
                animator.SetBool("ShowPrompt", false);
                canInteract = false;
                interactableNPC = null;
            }
        }

    }

    private void ManualNextSentence()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            dialogueManager.DisplayNextSentence();
        }
    }
}
