using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueTagTrigger : MonoBehaviour
{
    [SerializeField] private string[] texts;
    [SerializeField] private Vector3 textPos = new Vector3(0f, 1.8f, 0f);
    [SerializeField] private bool isAngry = false;
    private Transform textSocket;
    private int nextIndex = 0;
    private DialogueTagObj dialogueTagObj;
    void Start()
    {
        textSocket = new GameObject(gameObject.name + "Text Socket").transform;
        textSocket.position = transform.position + textPos;
    }

    public void Show()
    {
        if (texts.Length > 0)
        {
            if (dialogueTagObj != null)
                Destroy(dialogueTagObj.gameObject);
            dialogueTagObj = DialogueTag.instance.CreateDialogue(textSocket, texts[nextIndex], isAngry);
            nextIndex++;
            if (nextIndex == texts.Length)
                nextIndex = 0;
        }
    }

}
