﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneDataHolder : MonoBehaviour {

    [SerializeField] public SceneData data;

    private void Start()
    {
        Flag[] flags = FindObjectsOfType<Flag>();
        FastTravelLocation[] temp = new FastTravelLocation[GameSession.TotalCheckpointsInDifferentLevels[GameSession.currentLevel]];
        int tempint = 0;
        foreach(Flag f in flags)
        {
            temp[tempint] = f.location;
            tempint++;
        }
        data.fastTravelLocations = temp;
        MasterSceneData.allVisitedScenes.Add(data);
    }
}