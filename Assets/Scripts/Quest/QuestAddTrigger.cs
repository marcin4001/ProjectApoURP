using UnityEngine;

public class QuestAddTrigger : MonoBehaviour
{
    [SerializeField] private ActionDialogue actionQuest;
    [SerializeField] private int questID;
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
            Destroy(gameObject);
        }
    }
}
