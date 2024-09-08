using System.Collections.Generic;
using UnityEngine;

public class ListCabinet : MonoBehaviour
{
    public static ListCabinet instance;
    public List<CabinetItemList> list = new List<CabinetItemList>();
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

    public List<SlotItem> GetListItem(int id)
    {
        List<SlotItem> newList = new List<SlotItem>();
        CabinetItemList listItemLite = list.Find(x => x.idCabinet == id);
        if(listItemLite != null)
        {
            foreach(SlotItemLite slot in listItemLite.slots)
            {
                Item item = ItemDB.instance.GetItemById(slot.idItem);
                if(item != null )
                {
                    SlotItem slotItem = new SlotItem(item, slot.amount);
                    newList.Add(slotItem);
                }
            }
        }
        return newList;
    }

    public void SetListItem(int id, List<SlotItem> slots)
    {
        CabinetItemList oldList = list.Find(x => x.idCabinet == id);
        if (oldList != null)
            list.Remove(oldList);
        CabinetItemList newList = new CabinetItemList();
        newList.idCabinet = id;
        newList.slots = new List<SlotItemLite>();
        foreach (SlotItem slot in slots)
        {
            if (slot.GetItem() != null)
            {
                SlotItemLite slotLite = new SlotItemLite();
                slotLite.idItem = slot.GetItem().id;
                slotLite.amount = slot.GetAmount();
                newList.slots.Add(slotLite);
            }
        }
        list.Add(newList);
    }

    public void SaveCabinets()
    {
        Cabinet[] cabinets = FindObjectsByType<Cabinet>(FindObjectsSortMode.None);
        foreach(Cabinet cabinet in cabinets)
            cabinet.SaveItems();
    }
}
