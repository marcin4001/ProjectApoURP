
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    [SerializeField] private List<SlotItem> items = new List<SlotItem>();

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        
    }

    public void AddNonStackableItem(Item item)
    {
        SlotItem newSlot = new SlotItem(item, 1);
        items.Add(newSlot);
    }

    public void AddItem(Item item, int amount = 1)
    {
        SlotItem foundSlot = items.Find(slot => slot.GetItem() == item);
        if (foundSlot != null)
        {
            int newAmount = foundSlot.GetAmount() + amount;
            foundSlot.SetAmount(newAmount);
            return;
        }
        SlotItem newSlot = new SlotItem(item, amount);
        items.Add(newSlot);
    }

    public void AddItem(SlotItem item)
    {
        items.Add(item);
    }

    public void RemoveItem(SlotItem slot) 
    { 
        if (items.Contains(slot))
        {
            items.Remove(slot);
        }
    }

    public List<SlotItem> GetItems()
    { 
        return items; 
    }
}
