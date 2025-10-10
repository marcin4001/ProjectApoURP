using UnityEngine;

public class SwapObject : MonoBehaviour
{
    [SerializeField] private int questID;
    [SerializeField] private GameObject firstObj;
    [SerializeField] private GameObject secondObj;
    void Start()
    {
        if(QuestController.instance.HaveQuest(questID))
        {
            firstObj.SetActive(false);
        }
        else
        {
            secondObj.SetActive(false);
        }
    }

    
}
