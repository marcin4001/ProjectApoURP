using System.Collections.Generic;
using UnityEngine;

public class ItemDB : MonoBehaviour
{
    public static ItemDB instance;
    [SerializeField] private List<Item> items = new List<Item>();
    [SerializeField] private int testId = 0;

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

    public Item GetItemById(int id)
    {
        return items.Find(x => x.id == id);
    }

}
