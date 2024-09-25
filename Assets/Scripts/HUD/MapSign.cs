using UnityEngine;

[System.Serializable]
public class MapSign
{
    public string name;
    public MapSignState state;
}

public enum MapSignState
{
    Hidden,
    Unexplored,
    Explored
}
