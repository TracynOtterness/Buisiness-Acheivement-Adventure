using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastTravelButton : MonoBehaviour {
    GameSession gs;
	// Use this for initialization
	void Start () {
        gs = GameSession.gameSession;
	}

    public void TriggerWarp()
    {
        print("h");
        GameObject.Find("Warp Icon").GetComponent<Animator>().SetBool("Hiding", true);
        gs.Unpause();
        gs.HideUI();
        gs.StopCoroutines();
        gs.StartCoroutine(gs.FadeOut(3));
    }
}
