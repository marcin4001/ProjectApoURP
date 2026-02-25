using UnityEngine;

public class QuestDestroyNPCGroup : MonoBehaviour
{
    [SerializeField] private int questID;
    [SerializeField] private GameObject[] NPCs;
    [SerializeField] private GameObject[] items;
    [SerializeField] private GameObject[] itemsToShow;
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
    }

}
