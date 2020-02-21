using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gate")]
public class Gate : ScriptableObject {

    [SerializeField] public FastTravelLocation[] ftls;
    public SceneData[] scenes;

    private void Awake()
    {
        scenes = new SceneData[2] { ftls[0].nativeScene, ftls[1].nativeScene };
    }

}
