using UnityEngine;

[CreateAssetMenu(fileName = "MiscItem", menuName = "Item/MiscItem")]
public class MiscItem : Item
{
    public bool isAmmo = false;
    public bool isKey = false;
    public bool isBook = false;
    public BookProfile bookProfile;
    public bool addPointTechnical = false;
}
