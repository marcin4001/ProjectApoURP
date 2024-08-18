using System.Collections.Generic;
using UnityEngine;

public class QuestController : MonoBehaviour
{
    public static QuestController instance;
    [SerializeField] private List<Quest> quests = new List<Quest>();

    private void Awake()
    {
        instance = this;
    }

    public bool HaveQuest(int id)
    {
        bool result = quests.Exists(x => x.id == id);
        return result;
    }

    public void AddQuest(Quest quest)
    { 
        quests.Add(quest);
    }
}
