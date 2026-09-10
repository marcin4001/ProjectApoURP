using UnityEngine;

public class DeathNPCTrigger : MonoBehaviour
{
    [SerializeField] private int questID;
    [SerializeField] private GameObject npc;
    [SerializeField] private GameObject npcCorpse;
    void Start()
    {
        if(QuestController.instance.HaveQuest(questID))
        {
            npc.SetActive(false);
        }
        else
        {
            npcCorpse.SetActive(false);
        }
    }

}
