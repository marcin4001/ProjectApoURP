using UnityEngine;

[CreateAssetMenu(fileName = "MiscItem", menuName = "Item/MiscItem")]
public class MiscItem : Item
{
    public bool isAmmo = false;
    public bool isKey = false;
    public bool isBook = false;
    public BookProfile bookProfile;
    public bool addPointAttribute = false;
    public PlayerAttributes attribute = PlayerAttributes.strength;
}

public enum PlayerAttributes
{
    strength, dexterity, technical, perception
}
