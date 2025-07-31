using UnityEngine;

public class DialogueTag : MonoBehaviour
{
    public static DialogueTag instance;
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject dialoguePrefab;
    private RectTransform canvas;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        canvas = GetComponent<RectTransform>();
    }

    public Camera GetCamera()
    {
        return cam;
    }

    public RectTransform GetCanvasRect()
    {
        return canvas;
    }

    public DialogueTagObj CreateDialogue(Transform headPos, string text, bool isAngry = false)
    {
        GameObject newText = Instantiate(dialoguePrefab, canvas);
        DialogueTagObj dialogueTagObj = newText.GetComponent<DialogueTagObj>();
        if (dialogueTagObj != null)
        {
            dialogueTagObj.Init(headPos, text, isAngry);
            return dialogueTagObj;
        }
        return null;
    }

    //void Update()
    //{
    //    Vector3 posOnScreen = cam.WorldToScreenPoint(headNPC.position);
    //    Vector2 posText;
    //    RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, posOnScreen, null, out posText);
    //    tagText.anchoredPosition = posText;
    //}
}
