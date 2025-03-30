using UnityEngine;

public class RemoveAfterQuest : MonoBehaviour
{
    [SerializeField] private int questID = 0;
    void Start()
    {
        if(QuestController.instance != null)
        {
            if(QuestController.instance.Complete(questID))
                Destroy(gameObject);
        }
    }
}
