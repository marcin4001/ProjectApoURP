using UnityEngine;

public class PickupItem : MonoBehaviour, IUsableObj
{
    [SerializeField] private Item item;
    [SerializeField] private Transform nearPoint;
    public void Use()
    {
        bool added = HUDController.instance.AddItemToSlot(item);
        if (added)
            Destroy(gameObject);
    }

    public Vector3 GetNearPoint()
    {
        if(nearPoint == null)
            return transform.position;
        return nearPoint.position;
    }
}
