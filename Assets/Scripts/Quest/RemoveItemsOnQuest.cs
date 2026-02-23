using UnityEngine;

public class RemoveItemsOnQuest : MonoBehaviour
{
    [SerializeField] private int questID;
    [SerializeField] private GameObject item;
    void Start()
    {
        if(QuestController.instance.HaveQuest(questID))
        {
            Destroy(item);
        }
    }
}
