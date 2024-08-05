using UnityEngine;

public class PickupItem : MonoBehaviour, IUsableObj
{
    [SerializeField] private Item item;
    [SerializeField] private Transform nearPoint;
    public void Use()
    {
        if(item is WeaponItem)
        {
            Inventory.instance.AddNonStackableItem(item);
            Destroy(gameObject);
            return;
        }
        bool added = HUDController.instance.AddItemToHUDSlot(item);
        if (!added)
            Inventory.instance.AddItem(item);
        Destroy(gameObject);
    }

    public Vector3 GetNearPoint()
    {
        if(nearPoint == null)
            return transform.position;
        return nearPoint.position;
    }
}
