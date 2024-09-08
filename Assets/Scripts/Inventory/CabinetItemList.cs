using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CabinetItemList
{
    public int idCabinet;
    public List<SlotItemLite> slots = new List<SlotItemLite>();
}
