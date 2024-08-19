using UnityEngine;

[CreateAssetMenu(fileName = "CompleteQuestAction", menuName = "Dialogue/CompleteQuestAction")]
public class CompleteQuestAction : ActionDialogue
{
    public int idQuest;
    public override void Execute()
    {
        QuestController.instance.SetComplete(idQuest);
    }
}
