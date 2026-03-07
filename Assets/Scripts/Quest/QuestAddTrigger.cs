using UnityEngine;

public class QuestAddTrigger : MonoBehaviour
{
    [SerializeField] private ActionDialogue actionQuest;
    [SerializeField] private int questID;
    [SerializeField] private string playerDialogue;
    [SerializeField] private PlayerDialoguesList dialoguesList;
    void Start()
    {
        if(QuestController.instance.HaveQuest(questID))
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            actionQuest.Execute();
            if (dialoguesList != null)
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
