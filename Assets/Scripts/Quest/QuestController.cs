using System.Collections.Generic;
using UnityEngine;

public class QuestController : MonoBehaviour
{
    public static QuestController instance;
    [SerializeField] private List<Quest> quests = new List<Quest>();

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public bool HaveQuest(int id)
    {
        bool result = quests.Exists(x => x.id == id);
        return result;
    }

    public bool Complete(int id)
    {
        Quest quest = quests.Find(x => x.id == id);
        if(quest != null)
        {
            return quest.complete;
        }
        else
        {
            return false;
        }
    }

    public void SetComplete(int id)
    {
        Quest quest = quests.Find(x => x.id == id);
        if (quest != null)
        {
            quest.complete = true;
        }
    }

    public void AddQuest(Quest quest)
    { 
        Quest newQuest = new Quest(quest.id, quest.questTitle, quest.owner, quest.location);
        quests.Add(newQuest);
    }

    public List<Quest> GetQuests()
    {
        return quests;
    }
}
