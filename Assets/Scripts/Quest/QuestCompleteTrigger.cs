using UnityEngine;

public class QuestCompleteTrigger : MonoBehaviour
{
    public int idQuest;

    private void Start()
    {
        if(QuestController.instance.Complete(idQuest))
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            QuestController.instance.SetComplete(idQuest);
            Quest quest = QuestController.instance.GetQuest(idQuest);
            if (quest != null)
                HUDController.instance.SetQuestCompleteText(quest);
            Destroy(gameObject);
        }
    }
}
