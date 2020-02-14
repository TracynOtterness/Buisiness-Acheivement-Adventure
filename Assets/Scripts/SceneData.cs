using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SceneData")]
public class SceneData : ScriptableObject
{
    [SerializeField] public int buildIndex;
    [SerializeField] public int xCoordinate;
    [SerializeField] public int yCoordinate;
    [SerializeField] public FastTravelLocation[] fastTravelLocations;
}
