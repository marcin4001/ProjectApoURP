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
            if(quest.complete)
                return;
            quest.complete = true;
            GameParam.instance.AddExp(quest.exp);
        }
    }

    public void AddQuest(Quest quest)
    { 
        bool questOnList = quests.Exists(x => x.id == quest.id);
        if(questOnList)
            return;
        Quest newQuest = new Quest(quest.id, quest.questTitle, quest.owner, quest.location, quest.subQuests, quest.exp);
        quests.Add(newQuest);
        HUDController.instance.AddConsolelog("New quest added to journal");
    }

    public List<Quest> GetQuests()
    {
        return quests;
    }

    public Quest GetQuest(int id)
    {
        Quest quest = quests.Find(x => x.id == id);
        return quest;
    }

    public void ClearList()
    {
        quests.Clear();
    }
}
