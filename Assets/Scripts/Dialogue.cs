using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue{

    public string name;
    public Sprite avatar;
    [TextArea(4, 4)]
    public string[] sentences;
}
