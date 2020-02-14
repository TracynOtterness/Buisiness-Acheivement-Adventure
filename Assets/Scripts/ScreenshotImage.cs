using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenshotImage : MonoBehaviour {

    static Image image;

    private void Start()
    {
        image = GetComponent<Image>();
    }

    public static void SetImage(Sprite newSprite)
    {
        image.sprite = newSprite;
    }
}
