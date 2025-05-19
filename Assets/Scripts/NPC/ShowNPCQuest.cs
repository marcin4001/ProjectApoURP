using UnityEngine;

public class ShowNPCQuest : MonoBehaviour
{
    [SerializeField] private int questID = 0;
    void Start()
    {
        if (QuestController.instance != null)
        {
            if (!QuestController.instance.HaveQuest(questID))
                Destroy(gameObject);
        }
    }

}
