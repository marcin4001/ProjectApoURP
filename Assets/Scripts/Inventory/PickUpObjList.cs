using System.Collections.Generic;
using UnityEngine;

public class PickUpObjList : MonoBehaviour
{
    public static PickUpObjList instance;
    public List<string> objects; 
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public bool ExistOnList(string item)
    {
        return objects.Contains(item);
    }

    public void DestroyOnList(string item)
    {
        Debug.Log(objects.Contains(item));
        if (objects.Contains(item))
        {
            objects.Remove(item);
        }
    }
}
