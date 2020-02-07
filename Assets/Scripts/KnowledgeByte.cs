using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnowledgeByte : MonoBehaviour
{

    [SerializeField] AudioClip coinSound;
    [SerializeField] public Dialogue trivia;

    public KBUI kbui;

    bool hasBeenCollected;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasBeenCollected) { return; }
        print(this + " has been collected");
        kbui = PauseMenu.GetNextKBUI();
        QuestManager.UpdateQuestFlag("CollectAllKBs");

        hasBeenCollected = true;
        AudioSource.PlayClipAtPoint(coinSound, Camera.main.transform.position);
        FindObjectOfType<GameSession>().CollectKnowledgeByte(trivia);
        kbui.ToggleCollectionStatus(trivia);
        Destroy(gameObject);
    }
}
