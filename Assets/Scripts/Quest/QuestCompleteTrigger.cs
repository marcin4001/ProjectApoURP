using UnityEngine;

public class QuestCompleteTrigger : MonoBehaviour
{
    [SerializeField] private int idQuest;
    [SerializeField] private string playerDialogue;
    [SerializeField] private PlayerDialoguesList dialoguesList;

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
            if(dialoguesList != null)
            {
                dialoguesList.SpawnText();
                Destroy(gameObject);
                return;
            }
            if (!string.IsNullOrEmpty(playerDialogue))
                PlayerDialogues.instance.SetText(playerDialogue);
            Destroy(gameObject);
        }
    }
}
