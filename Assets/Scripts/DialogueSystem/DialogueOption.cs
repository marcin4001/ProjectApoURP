using UnityEngine;

[System.Serializable]
public class DialogueOption
{
    public string optionText;
    [TextArea(3,5)]
    public string replyText;
    public int nextNode = 0;
    public bool exitOption = false;
}
