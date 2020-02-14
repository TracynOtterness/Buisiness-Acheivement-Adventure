using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FastTravelUIButton : MonoBehaviour {

    public FastTravelLocation location;
    [SerializeField] Text text;

    public void SetName()
    {
        text.text = location.name;
    }

    public void PrimeForWarp()
    {
        GameSession.SetFTL(location);
        ScreenshotImage.SetImage(location.screenshot);
    }
}
