using System.Collections;
using UnityEngine;

public class QuestCompleteStart : MonoBehaviour
{
    [SerializeField] private int idQuest;

    void Start()
    {
        if (QuestController.instance.Complete(idQuest))
            Destroy(gameObject);
        StartCoroutine(CompleteQuest());
    }

    private IEnumerator CompleteQuest()
    {
        yield return new WaitForEndOfFrame();
        QuestController.instance.SetComplete(idQuest);
        Quest quest = QuestController.instance.GetQuest(idQuest);
        if (quest != null)
            HUDController.instance.SetQuestCompleteText(quest);
        Destroy(gameObject);
    }
}
