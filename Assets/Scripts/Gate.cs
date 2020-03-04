using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gate")]
public class Gate : ScriptableObject {

    [SerializeField] public FastTravelLocation[] ftls;
    [SerializeField] public SceneData[] scenes;
}
