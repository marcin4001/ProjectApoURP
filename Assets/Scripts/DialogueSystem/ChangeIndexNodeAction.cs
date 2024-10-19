using UnityEngine;

[CreateAssetMenu(fileName = "ChangeIndexNodeAction", menuName = "Dialogue/ChangeIndexNodeAction")]
public class ChangeIndexNodeAction : ActionDialogue
{
    public string nameNPC;
    public int index;
    public override void Execute()
    {
        NPCObjList.instance.SetIndexNode(nameNPC, index);
    }
}
