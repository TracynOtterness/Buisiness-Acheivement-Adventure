using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour {

    Animator animator;
    bool canWarp;
    public FastTravelLocation location;
    [SerializeField] string locationName;
    [SerializeField] Gate gate;

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
        location.name = this.locationName;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        canWarp = true;
        animator.SetBool("Hiding", false);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        canWarp = false;
        animator.SetBool("Hiding", true);
    }

    private void Update()
    {
        if (canWarp)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                animator.SetBool("Hiding", true);
                location.visited = true;
                GameSession.SetFTLByPortal(location, gate);
                GameSession.gameSession.StopCoroutines();
                GameSession.gameSession.StartCoroutine(GameSession.gameSession.FadeOut(3));
            }

        }
    }
}
