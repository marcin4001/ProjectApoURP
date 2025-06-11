using UnityEngine;

[CreateAssetMenu(fileName = "CompleteSubquestAction", menuName = "Dialogue/CompleteSubquestAction")]
public class CompleteSubquestAction : ActionDialogue
{
    public int questID = 0;
    public string subQuest;
    public override void Execute()
    {
        if (QuestController.instance.HaveQuest(questID))
        {
            Quest quest = QuestController.instance.GetQuest(questID);
            quest.SetCompleteSubQuest(subQuest);
        }
    }

}
