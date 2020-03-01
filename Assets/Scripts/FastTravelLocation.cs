using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FastTravelLocation")]
public class FastTravelLocation : ScriptableObject {

    [SerializeField] public Vector3 position;
    [SerializeField] public SceneData nativeScene;
    [SerializeField] public Sprite screenshot;
    [SerializeField] public bool visited;
	[SerializeField] public string locationName;
}
