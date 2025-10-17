using UnityEngine;

[CreateAssetMenu(fileName = "ArmorItem", menuName = "Item/ArmorItem")]
public class ArmorItem : Item
{
    public int defense;
    [Header("Clothes Material")]
    public Material clothes_top;
    public Material clothes_bottom;
    [Header("View Player")]
    public Sprite[] viewPlayerSprites;
}