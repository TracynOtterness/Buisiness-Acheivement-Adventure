using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CreateAssetMenu(menuName = "FastTravelLocation")]
public class FastTravelLocation : ScriptableObject {

    public Vector3 position;
    public SceneData nativeScene;
    [SerializeField] public Sprite screenshot;
    public bool visited;
	public string locationName;
}
