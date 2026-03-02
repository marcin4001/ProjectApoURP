using UnityEngine;

public class FinalCombatTrigger : MonoBehaviour
{
    [SerializeField] private int startQuestID = 1012;
    [SerializeField] private GameObject[] itemsToShow;
    [SerializeField] private bool disable = false;
    void Start()
    {
        if(disable)
        {
            return;
        }    
        if(QuestController.instance.HaveQuest(startQuestID))
            return;
        foreach(GameObject item in itemsToShow)
        {
            Destroy(item);
        }
    }

}
