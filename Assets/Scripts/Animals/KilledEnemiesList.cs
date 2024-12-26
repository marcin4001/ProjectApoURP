using System.Collections.Generic;
using UnityEngine;

public class KilledEnemiesList : MonoBehaviour
{
    public static KilledEnemiesList instance;
    public List<int> list = new List<int>();

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

    public bool OnList(int _id)
    {
        return list.Contains(_id);
    }

    public void AddToList(int _id)
    {
        if(_id < 0)
            return;
        if(!list.Contains(_id))
            list.Add(_id);
    }
}
