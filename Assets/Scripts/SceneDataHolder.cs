using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneDataHolder : MonoBehaviour {

    [SerializeField] public SceneData data;


    private void Awake()
    {
        if(data.name == "Null")
        {
            Debug.LogError("This Scene's SceneDataHolder hasn't been assigned properly!");
        }
        if (!MasterSceneData.allVisitedScenes.Contains(data))
        {
            print("Adding Scenedata from " + data.name);
            MasterSceneData.allVisitedScenes.Add(data);
        }
        else
        {
            print("We've already been to this scene!");
        }
    }

    private void Start()
    {
        data.buildIndex = SceneManager.GetActiveScene().buildIndex;
        Flag[] flags = FindObjectsOfType<Flag>();
        Portal[] portals = FindObjectsOfType<Portal>();
        FastTravelLocation[] temp = new FastTravelLocation[flags.Length + portals.Length];
        int tempint = 0;
        foreach(Flag f in flags)
        {
            temp[tempint] = f.location;
            tempint++;
        }
        foreach (Portal p in portals)
        {
            temp[tempint] = p.location;
            tempint++;
        }
        data.fastTravelLocations = temp;

    }
}