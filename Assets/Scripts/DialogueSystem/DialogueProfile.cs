using UnityEngine;

[CreateAssetMenu(fileName = "DialogueProfil", menuName = "Dialogue/DialogueProfile")]
public class DialogueProfile : ScriptableObject
{
    public string firstReply;
    public DialogueNode[] dialogueNodes;
}
