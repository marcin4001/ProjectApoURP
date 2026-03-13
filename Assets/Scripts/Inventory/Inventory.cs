
using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

[System.Serializable]
public class InventorySave
{
    public List<SlotItemLite> items;
    public SlotItemLite[] slots;
    public SlotItemLite armorSlot;
}

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    [SerializeField] private List<SlotItem> items = new List<SlotItem>();
    [SerializeField] private SlotItem[] slots = new SlotItem[3];
    [SerializeField] private SlotItem armorSlot;
    [SerializeField] private InventorySave inventorySave;

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
        if (item.GetItem() is ArmorItem)
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

    public SlotItem GetSlotItem(int index)
    {
        if(index < 0 || index >= slots.Length)
            return null;
        return slots[index];
    }

    public SlotItem GetArmorItem()
    {
        return armorSlot;
    }

    public int GetLengthSlotItem()
    {
        return slots.Length;
    }

    public void SetAmountSlot(int index, int amount)
    {
        if(index >= 0 && index < slots.Length)
            slots[index].SetAmount(amount);
    }

    public void SetNullSlot(int index)
    {
        if (index == -1)
        {
            armorSlot = new SlotItem(null, 0);
            return;
        }
        if (index >= 0 && index < slots.Length)
            slots[index] = new SlotItem(null, 0);
    }

    public void SetSlot(int index, SlotItem item)
    {
        if(index == -1)
        {
            armorSlot = item;
            return;
        }
        if (index >= 0 && index < slots.Length)
            slots[index] = item;
    }

    public SlotItem GetSlotById(int _id)
    {
        SlotItem slot = Array.Find(slots, x => x.GetItem().id == _id);
        if (slot != null)
            return slot;
        return new SlotItem(null, 0);
    }

    public SlotItem[] GetSlots()
    {
        return slots;
    }

    public void AddAmountToSlot(int index, int amount)
    {
        if (index >= 0 && index < slots.Length)
        {
            int newAmount = slots[index].GetAmount() + amount;
            slots[index].SetAmount(newAmount);
        }
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

    public void Clear()
    {
        items.Clear();
        for (int i = 0; i < slots.Length; i++)
            SetNullSlot(i);
    }

    public InventorySave Save()
    {
        inventorySave = new InventorySave();
        List<SlotItemLite> itemsLite = new List<SlotItemLite>();
        foreach(SlotItem item in items)
        {
            if(item.GetItem() != null)
            {
                SlotItemLite itemLite = new SlotItemLite();
                itemLite.idItem = item.GetItem().id;
                itemLite.amount = item.GetAmount();
                itemsLite.Add(itemLite);
            }
        }

        inventorySave.items = itemsLite;
        SlotItemLite[] slotsLite = new SlotItemLite[3];
        for (int i = 0;i < slots.Length;i++)
        {
            if(slots[i] != null && slots[i].GetItem() != null)
            {
                SlotItemLite itemLite = new SlotItemLite();
                itemLite.idItem = slots[i].GetItem().id;
                itemLite.amount = slots[i].GetAmount();
                slotsLite[i] = itemLite;
            }
            else
            {
                SlotItemLite itemLite = new SlotItemLite();
                itemLite.idItem = -1;
                itemLite.amount = 0;
                slotsLite[i] = itemLite;
            }
        }
        inventorySave.slots = slotsLite;
        if(armorSlot != null && armorSlot.GetItem() != null)
        {
            SlotItemLite armorSlotLite = new SlotItemLite();
            armorSlotLite.idItem = armorSlot.GetItem().id;
            armorSlotLite.amount = armorSlot.GetAmount();
            inventorySave.armorSlot = armorSlotLite;
        }
        else
        {
            SlotItemLite armorSlotLite = new SlotItemLite();
            armorSlotLite.idItem = -1;
            armorSlotLite.amount = 0;
            inventorySave.armorSlot = armorSlotLite;
        }

        return inventorySave;
    }

    public void Load(InventorySave save)
    {
        inventorySave = save;
        items = new List<SlotItem>();
        foreach(SlotItemLite item in inventorySave.items)
        {
            Item itemObj = ItemDB.instance.GetItemById(item.idItem);
            if(itemObj != null)
            {
                SlotItem slot = new SlotItem(itemObj, item.amount);
                items.Add(slot);
            }
        }

        slots = new SlotItem[3];
        for(int i = 0; i < slots.Length; i++)
        {
            Item itemObj = ItemDB.instance.GetItemById(inventorySave.slots[i].idItem);
            if(itemObj != null)
            {
                SlotItem slot = new SlotItem(itemObj, inventorySave.slots[i].amount);
                slots[i] = slot;
            }
            else
            {
                slots[i] = new SlotItem(null, 0);
            }
        }
        Item itemObjArmor = ItemDB.instance.GetItemById(inventorySave.armorSlot.idItem);
        if(itemObjArmor != null)
        {
            armorSlot = new SlotItem(itemObjArmor, inventorySave.armorSlot.amount);
        }
        else
        {
            armorSlot = new SlotItem(null, 0);
        }
    }

    
}
