using UnityEngine;

[System.Serializable]
public abstract class Item : ScriptableObject
{
    public int id = 0;
    public string nameItem = "";
    public Sprite uiSprite;
}

