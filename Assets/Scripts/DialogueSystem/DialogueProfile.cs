using UnityEngine;

[CreateAssetMenu(fileName = "DialogueProfil", menuName = "Dialogue/DialogueProfile")]
public class DialogueProfile : ScriptableObject
{
    public string npcName;
    public string job;
    public string location;
    public string firstReply;
    public DialogueNode[] dialogueNodes;
}
