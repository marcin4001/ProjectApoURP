using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    public static DialogueController instance;
    [SerializeField] private DialogueProfile profile;
    [SerializeField] private int indexNode = 0;
    [SerializeField] private bool init = false;
    private void Awake()
    {
        instance = this;
    }

    public void SetProfile(DialogueProfile _profile)
    {
        profile = _profile;
    }

    public void SetIndexNode(int _indexNode)
    {
        indexNode = _indexNode;
    }

    public void SetInit(bool _init)
    {
        init = _init;
    }

    public void ShowFirstDialogue()
    {
        if (profile == null)
            return;
        DialogueUI.instance.Show();
        DialogueUI.instance.SetNPCLabel(profile.npcName, profile.job, profile.location);
        if(!init)
            DialogueUI.instance.SetReply(profile.firstReply);
        else
            DialogueUI.instance.SetReply(profile.secReply);
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
        if (option.replyText != "")
        {
            DialogueUI.instance.SetReply(option.replyText);
        }
        indexNode = option.nextNode;
        CreateListOption();
    }

    private void CreateListOption()
    {
        DialogueOption[] options = profile.dialogueNodes[indexNode].options;
        List<DialogueOption> listOptions = new List<DialogueOption>();
        foreach (DialogueOption option in options)
        {
            if (option.IsMet())
            {
                listOptions.Add(option);
            }
        }
        DialogueUI.instance.CreateListOptions(listOptions);
    }

}
