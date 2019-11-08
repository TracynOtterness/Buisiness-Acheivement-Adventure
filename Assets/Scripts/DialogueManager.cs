using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{

    private Queue<string> sentences;

    [SerializeField] Text nameText;
    [SerializeField] Text sentenceText;
    [SerializeField] Image portrait;

    [SerializeField] float autoNextSentenceTime = 3f;
    [SerializeField] float nextCharacterWaitTime = .5f;
    public bool dialoging;

    bool typing;
    string currentSentence;
    Coroutine CR_TypewritterEffect;
    Coroutine CR_AutoNextSentence;

    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        if (dialoging) { return; }
        animator.SetBool("ShowDialogue", true);
        dialoging = true;

        nameText.text = dialogue.name;
        portrait.sprite = dialogue.avatar;

        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (typing)
        {
            StopCoroutine(CR_TypewritterEffect);
            typing = false;
            sentenceText.text = currentSentence;
            CR_AutoNextSentence = StartCoroutine(AutoNextSentence());
            return;
        }
        else if (CR_AutoNextSentence != null)
        {
            StopCoroutine(CR_AutoNextSentence);
        }
        if(sentences.Count == 0) { EndDialogue(); return; }
        sentenceText.text = "";
        currentSentence = sentences.Dequeue();
        StopAllCoroutines();
        CR_TypewritterEffect = StartCoroutine(TypewriterEffect());
    }

    IEnumerator TypewriterEffect()
    {
        typing = true;
        int initialSentenceLength = currentSentence.Length;
        int charsOnLine = 0;
        string iterationSentence = currentSentence;
        for (int i = 0; i < initialSentenceLength; i++)
        {
            sentenceText.text += iterationSentence[0];
            charsOnLine++;
            if (iterationSentence[0] == ' ') //behavior to prevent awkward text wrapping
            {
                int j = GetLettersInNextWord(iterationSentence);
                if (charsOnLine + j > 49)
                {
                    sentenceText.text += "\n";
                    iterationSentence = iterationSentence.Substring(1);
                    charsOnLine = 0;
                    continue;
                }
            }
            iterationSentence = iterationSentence.Substring(1);
            yield return new WaitForSeconds(.05f);
        }
        //legacy loop
        /*
        foreach (char c in currentSentence)
        {
            sentenceText.text += c;
            yield return new WaitForSeconds(.05f);
        }*/ 
        typing = false;
        CR_AutoNextSentence = StartCoroutine(AutoNextSentence());
    }

    private int GetLettersInNextWord(string iterationSentence)
    {
        int j;
        for (j = 1; j < iterationSentence.Length; j++)
        {
            if (iterationSentence[j] == ' ')
            {
                return j-1;
            }
        }
        return j-1;
    }

    IEnumerator AutoNextSentence()
    {
        yield return new WaitForSecondsRealtime(autoNextSentenceTime);
        DisplayNextSentence();
    }

    public void EndDialogue()
    {
        animator.SetBool("ShowDialogue", false);
        dialoging = false;
        sentences.Clear();
        //PromptNearbyNPCs(); replacing w/ animation event
    }

    private void PromptNearbyNPCs()
    {
        NPC[] npcs;
        InteractibilityIcon ii = FindObjectOfType<InteractibilityIcon>();
        npcs = FindObjectsOfType<NPC>();
        foreach(NPC n in npcs)
        {
            if (n.inPlayerRange)
            {
                ii.OpenInteractivityPrompt(n);
            }
        }
    }
}
