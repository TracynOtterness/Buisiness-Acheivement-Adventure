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
    [SerializeField] QuestPrompt questPrompt;
    [SerializeField] float longestCharWidth;

    [SerializeField] float autoNextSentenceTime = 3f;
    float nextCharacterWaitTime = .01f;
    public bool dialoging;

    bool typing;
    string currentSentence;
    Coroutine CR_TypewritterEffect;
    Coroutine CR_AutoNextSentence;

    Quest questToGive;

    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue, Quest quest)
    {
        if (dialoging) { return; }
        animator.SetBool("ShowDialogue", true);
        FindObjectOfType<Player>().controllable = false;

        questToGive = quest;
        dialoging = true;

        nameText.text = dialogue.name;
        portrait.sprite = dialogue.avatar;

        foreach (string sentence in dialogue.sentences)
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
        if (sentences.Count == 0) { EndDialogue(); return; }
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
                print(j);
                if(charsOnLine == 47)
                {
                    sentenceText.text += "\n";
                    iterationSentence = iterationSentence.Substring(1);
                    charsOnLine = 0;
                    continue;
                }
                if (charsOnLine + j > 46)
                {
                    sentenceText.text += "\n";
                    iterationSentence = iterationSentence.Substring(1);
                    charsOnLine = 0;
                    continue;
                }
                else
                {

                }
            }
            iterationSentence = iterationSentence.Substring(1);
            yield return new WaitForSeconds(nextCharacterWaitTime);
        }
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
                return j - 1;
            }
        }
        return j - 1;
    }

    IEnumerator AutoNextSentence()
    {
        yield return new WaitForSecondsRealtime(autoNextSentenceTime);
        DisplayNextSentence();
    }

    public void EndDialogue()
    {
        animator.SetBool("ShowDialogue", false);
        FindObjectOfType<Player>().controllable = true;
        dialoging = false;
        sentences.Clear();
    }

    public void GiveQuest()
    {
        if (questToGive != null && !QuestManager.activeQuests.Contains(questToGive))
        {
            questPrompt.SetupPrompt(questToGive);
        }
    }
}