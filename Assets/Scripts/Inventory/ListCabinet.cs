using System.Collections.Generic;
using UnityEngine;

public class ListCabinet : MonoBehaviour
{
    public static ListCabinet instance;
    public List<CabinetItemList> list = new List<CabinetItemList>();
    public CabinetPlaceData[] cabinetData;
    public List<ListCabinetData> cabinetsList = new List<ListCabinetData>();
    public int indexCabinetData = 0;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        foreach(CabinetPlaceData cabinetData in cabinetData)
        {
            ListCabinetData data = new ListCabinetData();
            data.list = cabinetData.Copy();
            cabinetsList.Add(data);
        }
    }


    public List<SlotItem> GetListItem(int id)
    {
        List<SlotItem> newList = new List<SlotItem>();
        Debug.Log(indexCabinetData);
        CabinetItemList listItemLite = cabinetsList[indexCabinetData].list.Find(x => x.idCabinet == id);
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
        else
        {
            Debug.Log("No on list");
        }
        return newList;
    }

    public void SetListItem(int id, List<SlotItem> slots)
    {
        CabinetItemList oldList = cabinetsList[indexCabinetData].list.Find(x => x.idCabinet == id);
        if (oldList != null)
            cabinetsList[indexCabinetData].list.Remove(oldList);
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
        cabinetsList[indexCabinetData].list.Add(newList);
    }

    public void SaveCabinets()
    {
        Cabinet[] cabinets = FindObjectsByType<Cabinet>(FindObjectsSortMode.None);
        foreach(Cabinet cabinet in cabinets)
            cabinet.SaveItems();
        VendingMachine[] vendingMachines = FindObjectsByType<VendingMachine>(FindObjectsSortMode.None);
        foreach(VendingMachine machine in vendingMachines)
            machine.SaveItems();
    }
}
