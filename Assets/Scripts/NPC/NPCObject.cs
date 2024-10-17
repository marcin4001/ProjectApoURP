using UnityEngine;

[System.Serializable]
public class NPCObject
{
    public string name;
    public bool init;
    public int nodeIndex;

    public NPCObject(string _name)
    {
        name = _name;
        init = false;
        nodeIndex = 0;
    }
}
