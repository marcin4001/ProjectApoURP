using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CabinetItemList
{
    public int idCabinet;
    public List<SlotItemLite> slots = new List<SlotItemLite>();

    public CabinetItemList Copy()
    {
        CabinetItemList copy = new CabinetItemList();
        copy.idCabinet = this.idCabinet;
        copy.slots = new List<SlotItemLite>();
        foreach (SlotItemLite slotItem in slots)
        {
            SlotItemLite slot = new SlotItemLite();
            slot.idItem = slotItem.idItem;
            slot.amount = slotItem.amount;
            copy.slots.Add(slot);
        }
        return copy;
    }
}
