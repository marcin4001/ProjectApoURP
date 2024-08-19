using UnityEngine;

[CreateAssetMenu(fileName = "QuestItemCondition", menuName = "Dialogue/QuestItemCondition")]
public class QuestItemCondition : ConditionDialogue
{
    public int idQuest;
    public bool negativeCondition = false;
    public SlotItem item;
    public override bool IsMet()
    {
        bool result = QuestController.instance.HaveQuest(idQuest);
        if(!result)
        {
            return result;
        }
        else
        {
            if (QuestController.instance.Complete(idQuest))
                return false;
            bool haveItem = Inventory.instance.PlayerHaveItem(item);
            if (negativeCondition)
                return !haveItem;
            return haveItem;
        }
    }
}
