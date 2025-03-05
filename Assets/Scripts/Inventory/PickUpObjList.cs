using System.Collections.Generic;
using UnityEngine;

public class PickUpObjList : MonoBehaviour
{
    public static PickUpObjList instance;
    public List<string> objects = new List<string>();
    public List<string> currentObjects = new List<string>();
    public List<int> objectIDs = new List<int>();
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        //CopyList();
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

    public void Clear()
    {
        objectIDs.Clear();
    }

    public void AddIdToList(int _id)
    {
        objectIDs.Add(_id);
        objectIDs.Sort();
    }

    public bool ExistOnList(int _id)
    {
        return objectIDs.Contains(_id);
    }
}
