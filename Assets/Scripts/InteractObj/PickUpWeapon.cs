using UnityEngine;

public class PickUpWeapon : MonoBehaviour, IUsableObj
{
    [SerializeField] private Item item;
    [SerializeField] private SlotItem ammoSlot;
    [SerializeField] private Transform nearPoint;
    public void Use()
    {
        Inventory.instance.AddNonStackableItem(item);
        if (ammoSlot != null && !ammoSlot.IsEmpty())
        {
            Inventory.instance.AddItem(ammoSlot.GetItem(), ammoSlot.GetAmount());
        }
        Destroy(gameObject);
        return;
    }

    public Vector3 GetNearPoint()
    {
        if (nearPoint == null)
            return transform.position;
        return nearPoint.position;
    }
}
