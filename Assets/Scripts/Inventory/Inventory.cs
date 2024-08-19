
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    [SerializeField] private List<SlotItem> items = new List<SlotItem>();

    private void Awake()
    {
        instance = this;
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
        if(item.GetItem() is WeaponItem)
        {
            SlotItem newItem = new SlotItem(item.GetItem(), 1);
            items.Add(newItem);
            return;
        }
        bool itemExist = items.Exists(x => x.GetItem().id == item.GetItem().id);
        if (itemExist)
        {
            SlotItem foundItem = items.Find(x => x.GetItem().id == item.GetItem().id);
            int newAmount = foundItem.GetAmount() + item.GetAmount();
            foundItem.SetAmount(newAmount);
            return;
        }
        SlotItem newItem2 = new SlotItem(item.GetItem(), item.GetAmount());
        items.Add(newItem2);
    }

    public void RemoveItem(SlotItem slot) 
    {
        bool itemExist = items.Exists(x => x.GetItem().id == slot.GetItem().id);
        if (itemExist)
        {
            SlotItem foundItem = items.Find(x => x.GetItem().id == slot.GetItem().id);
            int newAmount = foundItem.GetAmount() - slot.GetAmount();
            if (newAmount > 0)
            {
                foundItem.SetAmount(newAmount);
                return;
            }
            items.Remove(foundItem);
        }
    }

    public List<SlotItem> GetItems()
    { 
        return items; 
    }

    public SlotItem GetSlot(int _id) 
    {
        SlotItem slot = items.Find(x => x.GetItem().id == _id);
        if(slot != null)
            return slot;
        return new SlotItem(null, 0);
    }

    public bool PlayerHaveItem(int _id)
    {
        bool exist = items.Exists(x => x.GetItem().id == _id);
        return exist;
    }

    public bool PlayerHaveItem(SlotItem item)
    {
        SlotItem foundSlot = items.Find(x => x.GetItem().id == item.GetItem().id);
        if (foundSlot != null && foundSlot.GetAmount() >= item.GetAmount())
            return true;
        return false;
    }
}
