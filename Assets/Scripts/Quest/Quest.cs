using UnityEngine;

[System.Serializable]
public class Quest
{
    public int id;
    public string questTitle;
    public string owner;
    public string location;
    public bool complete = false;

    public Quest(int _id, string _title, string _owner, string _location)
    {
        id = _id;
        questTitle = _title;
        complete = false;
        owner = _owner;
        location = _location;
    }
}
