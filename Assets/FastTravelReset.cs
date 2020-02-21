using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastTravelReset : MonoBehaviour {

    [SerializeField] List<FastTravelLocation> fastTravelLocations;
    static FastTravelReset ftr;

    private void Awake()
    {
        if(ftr == null)
        {
            ftr = this;
            DontDestroyOnLoad(this.gameObject);
            ResetLocations();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void ResetLocations()
    {
        print("resetting visited status");
        if (fastTravelLocations.Count == 0)
        {
            print("FastTravelLocationReseter hasn't been set up!");
        }
        foreach (FastTravelLocation f in fastTravelLocations)
        {
            f.visited = false;
        }
    }

}
