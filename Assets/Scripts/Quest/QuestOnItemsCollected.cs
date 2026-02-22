using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestOnItemsCollected : MonoBehaviour
{
    [SerializeField] private List<Item> items = new List<Item>();
    [SerializeField] private int prevQuest;
    [SerializeField] private int questID;
    [SerializeField] private ActionDialogue actionDialogue;
    void Start()
    {
        if(actionDialogue == null)
            return;
        if(!QuestController.instance.HaveQuest(prevQuest))
            return;
        if(QuestController.instance.HaveQuest(questID))
            return;
        bool result = true;
        foreach(Item item in items)
        {
            result &= Inventory.instance.PlayerHaveItem(item.id);
        }
        if(result)
        {
            StartCoroutine(AddQuest());
        }
    }

    private IEnumerator AddQuest()
    {
        yield return new WaitForEndOfFrame();
        actionDialogue.Execute();
    }
}
