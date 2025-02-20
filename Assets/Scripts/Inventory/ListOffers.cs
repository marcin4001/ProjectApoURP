using System.Collections.Generic;
using UnityEngine;

public class ListOffers : MonoBehaviour
{
    public static ListOffers instance;
    public CabinetPlaceData initOffers;
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
        list = initOffers.Copy();
    }

    public List<SlotItem> GetListItem(int id)
    {
        List<SlotItem> newList = new List<SlotItem>();
        CabinetItemList listItemLite = list.Find(x => x.idCabinet == id);
        if (listItemLite != null)
        {
            foreach (SlotItemLite slot in listItemLite.slots)
            {
                Item item = ItemDB.instance.GetItemById(slot.idItem);
                if (item != null)
                {
                    SlotItem slotItem = new SlotItem(item, slot.amount);
                    newList.Add(slotItem);
                }
            }
        }
        else
        {
            Debug.Log("No on list");
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

    public void SaveOffers()
    {
        DialogueNPC[] nPCs = FindObjectsByType<DialogueNPC>(FindObjectsSortMode.None);
        foreach (DialogueNPC npc in nPCs)
        {
            npc.SaveItems();
        }
    }

    public void CopyList()
    {
        list = initOffers.Copy();
    }
}
