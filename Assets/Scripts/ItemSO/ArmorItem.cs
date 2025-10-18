using UnityEngine;

[CreateAssetMenu(fileName = "ArmorItem", menuName = "Item/ArmorItem")]
public class ArmorItem : Item
{
    public int defense;
    [Header("Clothes Material")]
    public Material clothes_top;
    public Material clothes_bottom;
    public Headgear headgear = Headgear.None;
    [Header("View Player")]
    public Sprite[] viewPlayerSprites;
}

public enum Headgear
{
    None,
    Helmet
}