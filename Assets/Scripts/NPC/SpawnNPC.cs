using UnityEngine;

public class SpawnNPC : MonoBehaviour
{
    [SerializeField] private GameObject npc;
    [SerializeField] private int questID;
    [SerializeField] private string nameNPC;
    void Start()
    {
        if (!QuestController.instance.HaveQuest(questID))
        {
            npc.SetActive(false);
            return;
        }
        if (NPCObjList.instance.isInit(nameNPC))
        {
            npc.SetActive(false);
        }
        
    }
}
