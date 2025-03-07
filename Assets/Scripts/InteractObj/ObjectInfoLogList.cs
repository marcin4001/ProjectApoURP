using System.Linq;
using UnityEngine;

public class ObjectInfoLogList : MonoBehaviour
{
    [SerializeField] private string[] logs;

    public void ShowLogs()
    {
        if(logs.Length == 0)
            return;
        HUDController.instance.AddConsolelog($"This is {GetArticle(logs[0])} {logs[0]}");
        if(logs.Length == 1)
            return;
        for(int i = 1; i < logs.Length; i++)
        {
            HUDController.instance.AddConsolelog(logs[i]);
        }
    }

    private string GetArticle(string log)
    {
        char[] chars = { 'a', 'e', 'i', 'o', 'u' };
        char firstLetter = log[0];
        if (chars.Contains(firstLetter))
            return "an";
        return "a";
    }
}
