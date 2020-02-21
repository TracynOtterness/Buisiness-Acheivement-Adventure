using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenshotImage : MonoBehaviour {

    static Image image;
    public static ScreenshotImage si;

    private void Awake()
    {
        if (si == null)
        {
            si = this;
        }
    }

    public void SetImage(Sprite newSprite)
    {
        image = GetComponent<Image>();
        image.sprite = newSprite;
    }
}
