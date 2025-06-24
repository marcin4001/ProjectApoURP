using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BookProfile", menuName = "Book/BookProfile")]
public class BookProfile : ScriptableObject
{
    public List<Page> pages = new List<Page>();
    public List<ActionDialogue> actions = new List<ActionDialogue>();

    public void Execute()
    {
        if(actions.Count == 0)
            return;
        foreach(ActionDialogue action in actions)
        {
            action.Execute();
        }
    }
}

[System.Serializable]
public class Page
{
    [TextArea(3,6)] public string text;
}