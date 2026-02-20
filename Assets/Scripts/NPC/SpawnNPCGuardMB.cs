using UnityEngine;

public class SpawnNPCGuardMB : MonoBehaviour
{
    [SerializeField] private GameObject npc;
    [SerializeField] private GameObject npcDeath;
    [SerializeField] private Item carBattery;
    [SerializeField] private Item fullCapsule;
    [SerializeField] private int questID;
    void Start()
    {
        if (!QuestController.instance.HaveQuest(questID))
        {
            npc.SetActive(false);
            npcDeath.SetActive(false);
            return;
        }
        if(Inventory.instance.PlayerHaveItem(carBattery.id) && Inventory.instance.PlayerHaveItem(fullCapsule.id))
        {
            npc.SetActive(false);
        }
        else
        {
            npcDeath.SetActive(false);
        }
    }
}
