using System.Collections.Generic;
using UnityEngine;

public class PickUpObjList : MonoBehaviour
{
    public static PickUpObjList instance;
    public List<string> objects = new List<string>();
    public List<string> currentObjects = new List<string>();
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        CopyList();
    }

    public bool ExistOnList(string item)
    {
        return currentObjects.Contains(item);
    }

    public void DestroyOnList(string item)
    {
        if (currentObjects.Contains(item))
        {
            currentObjects.Remove(item);
        }
    }

    public void CopyList()
    {
        if(currentObjects == null)
            currentObjects = new List<string>();
        else
            currentObjects.Clear();
        foreach (string item in objects)
            currentObjects.Add(item);   
    }
}
