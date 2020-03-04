using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastTravelReset : MonoBehaviour {

    [SerializeField] public List<FastTravelLocation> fastTravelLocations;
    public static FastTravelReset ftr;

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
            Debug.Log("FastTravelLocationReseter hasn't been set up!");
        }
        foreach (FastTravelLocation f in fastTravelLocations)
        {
            f.visited = false;
            if(f.locationName == null)
            {
                Debug.LogError("FastTravelLocation " + f.name + " does not have a location name!");
            }
            if (f.screenshot == null)
            {
                Debug.LogError("FastTravelLocation " + f.name + " does not have a screenshot!");
            }
        }
    }

}
