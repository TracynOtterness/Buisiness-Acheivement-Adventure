using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterSceneData : MonoBehaviour {

    public static List<SceneData> allVisitedScenes;
    List<Gate> allVisitedGates;

    public static MasterSceneData masterSceneData;

    private void Awake()
    {
        if (masterSceneData == null)
        {
            masterSceneData = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        allVisitedGates = new List<Gate>();
        allVisitedScenes = new List<SceneData>();
    }
}
