using UnityEngine;

public class NoticeBoard : MonoBehaviour, IUsableObj
{
    [SerializeField] private Transform nearPoint;
    [SerializeField] private SlotItem poster;
    [SerializeField] private bool active = false;
    [SerializeField] private int questID = 0;
    [SerializeField] private string subQuest;
    [SerializeField] private GameObject posterObj;
    [SerializeField] private GameObject center;
    private PlayerController player;

    private void Start()
    {
        active = false;
        if (QuestController.instance.HaveQuest(questID))
        {
            Quest quest = QuestController.instance.GetQuest(questID);
            if (quest.SubQuestIsComplete(subQuest))
                active = true;
        }
        player = FindFirstObjectByType<PlayerController>();
        if (!active && posterObj != null)
        {
            posterObj.SetActive(false);
        }
    }

    public bool CanUse()
    {
        return true;
    }

    public GameObject GetMainGameObject()
    {
        if(center != null)
            return center;
        return gameObject;
    }

    public Vector3 GetNearPoint()
    {
        return nearPoint.position;
    }

    public void Use()
    {
        if (active)
            return;
        if (!Inventory.instance.PlayerHaveItem(poster))
        {
            return;
        }
        Inventory.instance.RemoveItem(poster);
        posterObj.SetActive(true);
        active = true;
        if (QuestController.instance.HaveQuest(questID))
        {
            Quest quest = QuestController.instance.GetQuest(questID);
            quest.SetCompleteSubQuest(subQuest);
        }
    }
}
