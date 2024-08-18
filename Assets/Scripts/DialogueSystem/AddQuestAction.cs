using UnityEngine;

[CreateAssetMenu(fileName = "AddQuestAction", menuName = "Dialogue/AddQuestAction")]
public class AddQuestAction : ActionDialogue
{
    public Quest quest;
    public override void Execute()
    {
        QuestController.instance.AddQuest(quest);
    }
}
