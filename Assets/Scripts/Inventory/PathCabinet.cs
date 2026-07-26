using System.Collections.Generic;
using UnityEngine;

public class PathCabinet : MonoBehaviour
{
    [SerializeField] private List<CabinetItemList> list = new List<CabinetItemList>();
    
    public List<CabinetItemList> GetList()
    {
        return list;
    }
}
