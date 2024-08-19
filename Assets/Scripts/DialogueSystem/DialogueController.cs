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

    public void SetProfile(DialogueProfile _profile)
    {
        profile = _profile;
    }

    public void SetIndexNode(int _indexNode)
    {
        indexNode = _indexNode;
    }

    public void ShowFirstDialogue()
    {
        if (profile == null)
            return;
        DialogueUI.instance.Show();
        DialogueUI.instance.SetNPCLabel(profile.npcName, profile.job, profile.location);
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
            if (option.condition == null)
            {
                listOptions.Add(option);
                continue;
            }
            if(option.condition.IsMet())
                listOptions.Add(option);
        }
        DialogueUI.instance.CreateListOptions(listOptions);
    }

}
