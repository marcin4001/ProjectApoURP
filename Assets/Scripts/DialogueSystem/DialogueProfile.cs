using UnityEngine;

[CreateAssetMenu(fileName = "DialogueProfil", menuName = "Dialogue/DialogueProfile")]
public class DialogueProfile : ScriptableObject
{
    public string npcName;
    public string job;
    public string location;
    [TextArea(3, 5)]
    public string firstReply;
    [TextArea(3, 5)]
    public string secReply;
    public DialogueNode[] dialogueNodes;
}
