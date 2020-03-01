using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterSceneData : MonoBehaviour {

    public static List<SceneData> allVisitedScenes;
    public static List<Gate> allVisitedGates;

    public static MasterSceneData masterSceneData;

    private void Awake()
    {
        if (masterSceneData == null)
        {
            masterSceneData = this;
            DontDestroyOnLoad(gameObject);
            allVisitedGates = new List<Gate>();
            allVisitedScenes = new List<SceneData>();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
