using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterSceneData : MonoBehaviour {

    public static List<SceneData> allVisitedScenes;
    List<Gate> allVisitedGates;

    private void Start()
    {
        allVisitedGates = new List<Gate>();
        allVisitedScenes = new List<SceneData>();
    }
}
