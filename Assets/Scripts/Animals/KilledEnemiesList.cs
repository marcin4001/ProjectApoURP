using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class KilledEnemiesListSave
{
    public List<int> list = new List<int>();
}

public class KilledEnemiesList : MonoBehaviour
{
    public static KilledEnemiesList instance;
    public List<int> list = new List<int>();
    public List<KilledEnemiesGroup> groups = new List<KilledEnemiesGroup>();

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

    public bool OnList(int _id)
    {
        return list.Contains(_id);
    }

    public void AddToList(int _id)
    {
        if(_id < 0)
            return;
        if(!list.Contains(_id))
            list.Add(_id);
    }

    public bool IsGroupDefeated(int idQuest)
    {
        KilledEnemiesGroup group = groups.Find(x => x.idQuest == idQuest);
        if(group == null)
            return true;
        if(group.enemies.Count == 0)
            return true;
        foreach(int enemy in group.enemies)
        {
            if(!OnList(enemy)) return false;
        }
        return true;
    }

    public void ClearList()
    {
        list.Clear();
    }

    public KilledEnemiesListSave GetSaveObj()
    {
        KilledEnemiesListSave obj = new KilledEnemiesListSave();
        obj.list = list;
        return obj;
    }

    public void Load(KilledEnemiesListSave save)
    {
        list.Clear();
        list = save.list;
    }
}

[System.Serializable]
public class KilledEnemiesGroup
{
    public int idQuest = 0;
    public List <int> enemies = new List<int>();
}
