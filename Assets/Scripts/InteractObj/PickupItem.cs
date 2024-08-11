using UnityEngine;

public class PickupItem : MonoBehaviour, IUsableObj
{
    [SerializeField] private Item item;
    [SerializeField] private int amount = 1;
    [SerializeField] private Transform nearPoint;
    public void Use()
    {
        //bool added = HUDController.instance.AddItemToHUDSlot(item, amount);
        //if (!added)
        Inventory.instance.AddItem(item, amount);
        if (item is MiscItem)
        {
            MiscItem miscItem = (MiscItem)item;
            if (miscItem.isAmmo)
            {
                WeaponObject weapon = WeaponController.instance.GetWeaponByAmmo(item.id);
                if (weapon != null)
                {
                    weapon.UpdateAmmoOutGun();
                }
            }
        }
        Destroy(gameObject);
    }

    public Vector3 GetNearPoint()
    {
        if(nearPoint == null)
            return transform.position;
        return nearPoint.position;
    }

    public GameObject GetMainGameObject()
    {
        return gameObject;
    }
}
