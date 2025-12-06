using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapCombatController : MonoBehaviour
{
    public static MapCombatController instance;
    [SerializeField] private CellGridMapCombat[] gridCombats;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        gridCombats = FindObjectsByType<CellGridMapCombat>(FindObjectsSortMode.None);
        foreach(CellGridMapCombat grid in gridCombats)
        {
            grid.SetIsCombat(false);
        }
        List<CellGridMapCombat> pool = gridCombats.ToList();
        List<CellGridMapCombat> select = new List<CellGridMapCombat>();

        int maxSelect = pool.Count/2 + 1;
        Debug.Log(maxSelect);
        for (int i = 0; i < maxSelect; i++)
        {
            int randomIndex = Random.Range(0, pool.Count);
            select.Add(pool[randomIndex]);
            pool.RemoveAt(randomIndex);
        }

        foreach(CellGridMapCombat grid in select)
        {
            Debug.Log(grid.gameObject.name);
            grid.SetIsCombat(true);
        }
    }

    public bool CheckTriggers()
    {
        if(gridCombats == null) return false;
        if(gridCombats.Length == 0) return false;
        foreach(CellGridMapCombat cell in gridCombats)
        {
            if(cell == null) continue;
            if(cell.TriggerCombat())
                return true;
        }
        return false;
    }
}
