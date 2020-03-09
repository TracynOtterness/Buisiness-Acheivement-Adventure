using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class KBUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler{

    [SerializeField] public int order;

    static Text nameText;
    static Text knowledgeText;
    static GameObject miniDialogueBox;

    static KBUI referenceSetter;

    static float adjustmentValueX = 5f;
    static float adjustmentValueY = 10f;

    public Dialogue trivia;
    bool collected;

    private void Awake()
    {
        if(referenceSetter == null)
        {
            referenceSetter = this;
        }
        else
        {
            return;
        }
        miniDialogueBox = GameObject.Find("MiniDialogueBox");
        nameText = GameObject.Find("MiniDialogueNameText").GetComponent<Text>();
        knowledgeText = GameObject.Find("MiniDialogueKnowledgeText").GetComponent<Text>();
    }

    private void Start()
    {
        /*if(this == referenceSetter)
        {
            print(nameText);
            print(knowledgeText);
            print(miniDialogueBox);
        }*/
    }

    public void ToggleCollectionStatus(Dialogue passedTrivia)
    {
        collected = true;
        trivia = passedTrivia;
        GetComponent<Image>().color = Color.white;
        PauseMenu.IncreaseKBCount();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        print("OnPointerEnter on + " + gameObject);
        if (collected)
        {
            nameText.text = trivia.name;
            knowledgeText.text = trivia.sentences[0];
            Vector3 targetPosition = new Vector3(transform.position.x + Screen.width / adjustmentValueX, transform.position.y + Screen.height / adjustmentValueY, transform.position.z);
            miniDialogueBox.transform.position = targetPosition;
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        miniDialogueBox.transform.position = new Vector3(Screen.height, Screen.width, 0f);
    }
}
