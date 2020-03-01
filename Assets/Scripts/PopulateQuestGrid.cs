using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulateQuestGrid : MonoBehaviour {


    [SerializeField] GameObject prefab;
    [SerializeField] int numberToCreate;
    [SerializeField] Text selectedText;

    public void PopulateGrid()
    {
        DeleteOldObjects();
        SetSelectedTextToDefault();
        int xCoord = 0;
        int yCoord = 4;
        for (int i = 0; i < numberToCreate; i++)
        {
            GameObject newObject = Instantiate(prefab); // Create GameObject instance
            newObject.transform.SetParent(gameObject.transform, false);
            ZoomedOutMinimapButton zomb = newObject.GetComponent<ZoomedOutMinimapButton>();
            zomb.Setup(xCoord, yCoord, selectedText);
            xCoord++;
            if (xCoord == 5)
            {
                xCoord = 0;
                yCoord--;
            }
        }
    }
    void DeleteOldObjects()
    {
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
    void SetSelectedTextToDefault()
    {
        int currentSceneXCoord = FindObjectOfType<SceneDataHolder>().data.xCoordinate;
        int currentSceneYCoord = FindObjectOfType<SceneDataHolder>().data.yCoordinate;
        selectedText.text = currentSceneXCoord + "," + currentSceneYCoord;
    }
}
