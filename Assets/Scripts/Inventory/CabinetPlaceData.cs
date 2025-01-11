using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CabinetPlaceData", menuName = "Cabinet/CabinetPlaceData")]
public class CabinetPlaceData : ScriptableObject
{
    public List<CabinetItemList> list = new List<CabinetItemList>();

    public List<CabinetItemList> Copy()
    {
        List<CabinetItemList> copy = new List<CabinetItemList>();
        foreach(CabinetItemList item in list)
        {
            CabinetItemList coptItem = item.Copy();
            copy.Add(coptItem);
        }
        return copy;
    }

}
