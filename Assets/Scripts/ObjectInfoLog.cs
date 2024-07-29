using UnityEngine;
using System.Linq;

public class ObjectInfoLog : MonoBehaviour
{
    [SerializeField] private string nameObject;

    public string GetLog()
    {
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
}
