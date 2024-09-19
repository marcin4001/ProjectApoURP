using UnityEngine;

[System.Serializable]
public class DialogueOption
{
    public string optionText;
    [TextArea(3,5)]
    public string replyText;
    public int nextNode = 0;
    public bool exitOption = false;
    public ConditionDialogueType conditionType = ConditionDialogueType.None;
    public int questID = 0;
    public int objectID = 0;
    public int amountObject = 0;
    public ActionDialogue[] actions;

    public bool IsMet()
    {
        switch(conditionType)
        {
            case ConditionDialogueType.None:
                return true;
            case ConditionDialogueType.QuestNotStarted:
                return QuestNotStarted();
            case ConditionDialogueType.QuestComplete: 
                return QuestComplete();
            case ConditionDialogueType.PlayerNoHaveItem:
                return PlayerNoHaveItem();
            case ConditionDialogueType.PlayerHaveItem:
                return PlayerHaveItem();
        }
        return true;
    }

    private bool QuestNotStarted()
    {
        if(QuestController.instance == null)
            return false;
        return !QuestController.instance.HaveQuest(questID);
    }

    private bool QuestComplete()
    {
        if (QuestController.instance == null)
            return false;
        return QuestController.instance.Complete(questID);
    }

    private bool PlayerNoHaveItem()
    {
        if (QuestController.instance == null)
            return false;
        if(QuestController.instance.HaveQuest(questID))
        {
            if (QuestController.instance.Complete(questID))
                return false;
            if(Inventory.instance == null || ItemDB.instance == null)
                return true;
            Item item = ItemDB.instance.GetItemById(objectID);
            if (item == null)
                return true;
            SlotItem slot = new SlotItem(item, amountObject);
            return !Inventory.instance.PlayerHaveItem(slot);
        }
        return false;
    }

    private bool PlayerHaveItem()
    {
        if (QuestController.instance == null)
            return false;
        if (QuestController.instance.HaveQuest(questID))
        {
            if (QuestController.instance.Complete(questID))
                return false;
            if (Inventory.instance == null || ItemDB.instance == null)
                return false;
            Item item = ItemDB.instance.GetItemById(objectID);
            if (item == null)
                return false;
            SlotItem slot = new SlotItem(item, amountObject);
            return Inventory.instance.PlayerHaveItem(slot);
        }
        return false;
    }
}

public enum ConditionDialogueType
{
    None,
    QuestNotStarted,
    QuestComplete,
    PlayerNoHaveItem,
    PlayerHaveItem,
}
