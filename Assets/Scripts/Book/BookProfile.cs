using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BookProfile", menuName = "Book/BookProfile")]
public class BookProfile : ScriptableObject
{
    public List<Page> pages = new List<Page>();
}

[System.Serializable]
public class Page
{
    [TextArea(3,6)] public string text;
}