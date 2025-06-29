using System;
using UnityEngine;

public class ChangeIndexNodeTrigger : MonoBehaviour
{
    [SerializeField] private int questID = 0;
    [SerializeField] private string nameNPC;
    [SerializeField] private int newIndex = 0;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if(QuestController.instance.Complete(questID))
            {
                Destroy(gameObject);
                return;
            }
            if (QuestController.instance.HaveQuest(questID))
            {
                NPCObjList.instance.SetIndexNode(nameNPC, newIndex);
                Destroy(gameObject);
            }
        }
    }
}
