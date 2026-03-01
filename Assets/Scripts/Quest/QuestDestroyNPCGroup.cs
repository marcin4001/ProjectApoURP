using UnityEngine;

public class QuestDestroyNPCGroup : MonoBehaviour
{
    [SerializeField] private int questID;
    [SerializeField] private int questIDNext;
    [SerializeField] private GameObject[] NPCs;
    [SerializeField] private GameObject[] items;
    [SerializeField] private GameObject[] itemsToShow;
    [SerializeField] private GameObject carToDestroy;
    void Start()
    {
        if(!QuestController.instance.HaveQuest(questID))
        {
            foreach(GameObject item in itemsToShow)
            {
                Destroy(item);
            }
            return;
        }
        foreach(GameObject npc in NPCs)
        {
            Destroy(npc);
        }
        foreach (GameObject item in items)
        {
            Destroy(item);
        }
        if(QuestController.instance.HaveQuest(questIDNext))
        {
            Destroy(carToDestroy);
        }
    }

}
