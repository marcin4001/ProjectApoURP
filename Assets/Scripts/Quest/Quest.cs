using UnityEngine;

[System.Serializable]
public class Quest
{
    public int id;
    public string questTitle;
    public bool complete = false;

    public Quest(int _id, string _title)
    {
        id = _id;
        questTitle = _title;
        complete = false;
    }
}
