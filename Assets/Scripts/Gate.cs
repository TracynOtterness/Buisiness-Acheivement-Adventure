using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour {

    [SerializeField] public FastTravelLocation[] portals;
    public SceneData[] scenes;

    private void Awake()
    {
        scenes = new SceneData[2] { portals[0].nativeScene, portals[1].nativeScene };
    }

}
