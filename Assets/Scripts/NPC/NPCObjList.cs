using System.Collections.Generic;
using UnityEngine;

public class NPCObjList : MonoBehaviour
{
    public static NPCObjList instance;
    [SerializeField]
    private List<NPCObject> list = new List<NPCObject>();
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

    public void AddNPC(string _name)
    {
        bool exist = list.Exists(x  => x.name == _name);
        if(exist)
            return;
        NPCObject newNPC = new NPCObject(_name);
        list.Add(newNPC);
    }

    public bool isInit(string _name)
    {
        NPCObject npc = list.Find(x => x.name == _name);
        if(npc != null)
        {
            return npc.init;
        }
        return false;
    }

    public void SetInit(string _name)
    {
        NPCObject npc = list.Find(x => x.name == _name);
        if (npc != null)
        {
            npc.init = true;
        }
    }

    public int GetIndexNode(string _name)
    {
        NPCObject npc = list.Find(x => x.name == _name);
        if (npc != null)
        {
            return npc.nodeIndex;
        }
        return 0;
    }

    public void SetIndexNode(string _name, int index)
    {
        NPCObject npc = list.Find(x => x.name == _name);
        if (npc != null)
        {
            npc.nodeIndex = index;
        }
    }

    public void ClearList()
    {
        list.Clear();
    }
}
