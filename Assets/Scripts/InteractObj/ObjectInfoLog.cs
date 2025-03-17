using UnityEngine;
using System.Linq;

public class ObjectInfoLog : MonoBehaviour
{
    [SerializeField] private string nameObject;
    [SerializeField] private bool isNPC;

    public string GetLog()
    {
        if(isNPC)
            return $"This is {nameObject}";
        return $"This is {GetArticle()} {nameObject}";
    }

    private string GetArticle()
    {
        char[] chars = { 'a', 'e', 'i', 'o', 'u' };
        char firstLetter = nameObject[0];
        if (chars.Contains(firstLetter))
            return "an";
        return "a";
    }

    public void SetNameObject(string _name)
    {
        nameObject = _name;
    }
}
