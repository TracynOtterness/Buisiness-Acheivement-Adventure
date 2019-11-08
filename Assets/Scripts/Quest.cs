using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest {

    [SerializeField] public string questName;
    [SerializeField] public Sprite avatar;
    [TextArea(4, 4)]
    [SerializeField] public string questDescription;

}
