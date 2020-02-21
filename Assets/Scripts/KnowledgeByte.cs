using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnowledgeByte : MonoBehaviour
{

    [SerializeField] AudioClip coinSound;
    [SerializeField] public Dialogue trivia;
    [Range(0f, 1f)]
    [SerializeField] float volume;
    [SerializeField] GameObject cosaPrefab;
    public KBUI kbui;

    bool hasBeenCollected;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasBeenCollected) { return; }
        print(this + " has been collected");
        kbui = PauseMenu.GetNextKBUI();

        hasBeenCollected = true;
        CustomOneShotAudio cosa = Instantiate(cosaPrefab, Camera.main.transform).GetComponent<CustomOneShotAudio>();
        cosa.PlayAudio(coinSound, volume);
        FindObjectOfType<GameSession>().CollectKnowledgeByte(trivia);
        kbui.ToggleCollectionStatus(trivia);
        Destroy(gameObject);
    }
}
