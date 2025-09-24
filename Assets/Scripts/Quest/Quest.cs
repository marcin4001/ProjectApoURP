using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public int id;
    public string questTitle;
    public string owner;
    public string location;
    public bool complete = false;
    public List<SubQuest> subQuests = new List<SubQuest>();
    public int exp = 0;
    public bool hidden = false;

    public Quest(int _id, string _title, string _owner, string _location, List<SubQuest> _subQuests, int exp, bool hidden)
    {
        id = _id;
        questTitle = _title;
        complete = false;
        owner = _owner;
        location = _location;
        subQuests = new List<SubQuest>();
        foreach (SubQuest subQuest in _subQuests)
        {
            SubQuest newSubQuest = new SubQuest();
            newSubQuest.title = subQuest.title;
            newSubQuest.complete = subQuest.complete;
            subQuests.Add(newSubQuest);
        }

        this.exp = exp;
        this.hidden = hidden;
    }

    public bool CanComplete()
    {
        if(subQuests ==  null)
            return true;
        if (subQuests.Count == 0)
            return true;
        foreach(SubQuest subQuest in subQuests)
        {
            if(!subQuest.complete)
                return false;
        }
        return true;
    }

    public bool SubQuestIsComplete(string subQuest)
    {
        if (subQuests == null)
            return false;
        if (subQuests.Count == 0)
            return false;
        SubQuest subQuestObj = subQuests.Find(x => x.title == subQuest);
        if(subQuestObj != null)
            return subQuestObj.complete;
        return false;
    }

    public void SetCompleteSubQuest(string subQuest)
    {
        if (subQuests == null)
            return;
        if (subQuests.Count == 0)
            return;
        SubQuest subQuestObj = subQuests.Find(x => x.title == subQuest);
        if (subQuestObj != null)
            subQuestObj.complete = true;
    }
}

[System.Serializable]
public class SubQuest
{
    public string title;
    public bool complete = false;
}
