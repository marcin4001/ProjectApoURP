using UnityEngine;

[System.Serializable]
public abstract class Item : ScriptableObject
{
    public int id = 0;
    public string nameItem = "";
    [TextArea(3,7)]
    public string description = "";
    public Sprite uiSprite;
    public int value;
}

