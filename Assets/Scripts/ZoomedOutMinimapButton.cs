using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZoomedOutMinimapButton : MonoBehaviour {

    public int xCoord;
    public int yCoord;
    public Text selectedText;

    [SerializeField] Text text;
	public void Setup(int x, int y, Text textToUpdate)
    {
        xCoord = x;
        yCoord = y;
        selectedText = textToUpdate;
        HideIfNotVisited();
    }

    void HideIfNotVisited()
    {
        Image image = GetComponent<Image>();
        Button button = GetComponent<Button>();
        image.enabled = false;
        button.interactable = false;
        text.text = "";
        bool visited = false;
        foreach(SceneData sd in MasterSceneData.allVisitedScenes)
        {
            if(sd.xCoordinate == xCoord && sd.yCoordinate == yCoord)
            {
                print("visited");
                visited = true;
                image.enabled = true;
                button.interactable = true;
                text.text = xCoord + "," + yCoord;
                return;
            }
        }
    }

    public void UpdateSelectedText()
    {
        PauseMenu.ZoomedOutMapXCoord = xCoord;
        PauseMenu.ZoomedOutMapYCoord = yCoord;
        PauseMenu.pauseMenu.PopulateFastTravelLocations();
        selectedText.text = xCoord + "," + yCoord;
    }
}
