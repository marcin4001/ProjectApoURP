using UnityEngine;

[CreateAssetMenu(fileName = "WeaponItem", menuName = "Item/WeaponItem")]
public class WeaponItem : Item
{
    public int damage = 0;
    public int ammoMax = 0;
    public int idAmmo = 0;
    public WeaponType type;
    public Vector3 positionInHand = Vector3.zero;
    public Vector3 rotationInHand = Vector3.zero;
    public Vector3 scaleInHand = Vector3.one;
}

public enum WeaponType
{
    Rifle, HandGun, Melee
}
