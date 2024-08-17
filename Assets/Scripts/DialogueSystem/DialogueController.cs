using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    public static DialogueController instance;
    [SerializeField] private DialogueProfile profile;
    [SerializeField] private int indexNode = 0;
    private void Awake()
    {
        instance = this;
    }

    public void ShowFirstDialogue()
    {
        DialogueUI.instance.Show();
        DialogueUI.instance.SetReply(profile.firstReply);
        CreateListOption();
    }

    public void ShowNextDialogue(DialogueOption option)
    {
        if(option.exitOption)
        {
            DialogueUI.instance.Hide();
            indexNode = 0;
            return;
        }
        DialogueUI.instance.SetReply(option.replyText);
        indexNode = option.nextNode;
        CreateListOption();
    }

    private void CreateListOption()
    {
        DialogueOption[] options = profile.dialogueNodes[indexNode].options;
        List<DialogueOption> listOptions = new List<DialogueOption>();
        foreach (DialogueOption option in options)
        {
            listOptions.Add(option);
        }
        DialogueUI.instance.CreateListOptions(listOptions);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            ShowFirstDialogue();
        }
    }
}
