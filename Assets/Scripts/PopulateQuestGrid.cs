using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulateQuestGrid : MonoBehaviour {


    [SerializeField] GameObject prefab;
    [SerializeField] int numberToCreate;
    // Use this for initialization
    void Start()
    {
        for(int i = 0; i < numberToCreate; i++)
        {
            GameObject newObject = Instantiate(prefab); // Create GameObject instance
            newObject.transform.SetParent(gameObject.transform, false);
        }

    }
}
