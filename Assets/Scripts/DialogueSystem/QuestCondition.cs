using UnityEngine;

[CreateAssetMenu(fileName = "QuestCondition", menuName = "Dialogue/QuestCondition")]
public class QuestCondition : ConditionDialogue
{
    public int idQuest;
    public bool negativeCondition = false;
    public override bool IsMet()
    {
        bool result = QuestController.instance.HaveQuest(idQuest);
        if(negativeCondition)
            return !result;
        return result;
    }
}
