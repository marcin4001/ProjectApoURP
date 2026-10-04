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

    public List<Item> GetWeaponItems()
    {
        return items.FindAll(x => x is WeaponItem);
    }

    public void AddAllItemToInventory()
    {
        List<Item> itemsR = new List<Item>(items);
        itemsR.Reverse();
        foreach(Item item in itemsR)
        {
            SlotItem slot = new SlotItem(item, 1);
            Inventory.instance.AddItem(slot);
        }
    }
}
