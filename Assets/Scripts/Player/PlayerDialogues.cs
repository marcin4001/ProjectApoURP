using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerDialogues : MonoBehaviour
{
    public static PlayerDialogues instance;
    [SerializeField] private TextMeshPro dialogueText;
    [SerializeField] private float timeToHide = 3f;
    [SerializeField] private Vector3 textPos = new Vector3(0f, 1.8f, 0f);
    private Transform textSocket;
    private DialogueTagObj dialogueTagObj;
    private PlayerController player;
    private Coroutine coroutine;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        dialogueText.gameObject.SetActive(false);
        textSocket = new GameObject(gameObject.name + "Text Socket").transform;
        textSocket.position = transform.position + textPos;
        player = GetComponent<PlayerController>();
    }

    public void SetText(string dialogueText)
    {
        player.StopMove();
        player.SetBlock(true);
        textSocket = new GameObject(gameObject.name + "Text Socket").transform;
        textSocket.position = transform.position + textPos;
        if (DialogueTag.instance != null)
        {
            if (dialogueTagObj != null)
                Destroy(dialogueTagObj.gameObject);
            dialogueTagObj = DialogueTag.instance.CreateDialogue(textSocket, dialogueText, false);
            if(coroutine != null)
                StopCoroutine(coroutine);
            coroutine = StartCoroutine(HideText());
        }
    }

    private IEnumerator HideText()
    {
        yield return new WaitForSeconds(timeToHide);
        player.SetBlock(false);
    }

    //void Update()
    //{
    //    if(Input.GetKeyDown(KeyCode.K))
    //    {
    //        SetText("Hello there");
    //    }
    //}
}
