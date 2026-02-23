using UnityEngine;

public class QuestChainTrigger : MonoBehaviour
{
    [SerializeField] private int questID;
    [SerializeField] private ActionDialogue actionDialogue;
    void Start()
    {
        if(!QuestController.instance.HaveQuest(questID))
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            actionDialogue?.Execute();
        }
    }
}
