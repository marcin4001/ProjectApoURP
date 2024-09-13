using UnityEngine;

public class PickUpWeapon : MonoBehaviour, IUsableObj
{
    [SerializeField] private Item item;
    [SerializeField] private SlotItem ammoSlot;
    [SerializeField] private Transform nearPoint;

    private void Start()
    {
        bool result = PickUpObjList.instance.ExistOnList(gameObject.name);
        if (!result)
            Destroy(gameObject);
    }
    public void Use()
    {
        Inventory.instance.AddNonStackableItem(item);
        if (ammoSlot != null && !ammoSlot.IsEmpty())
        {
            Inventory.instance.AddItem(ammoSlot.GetItem(), ammoSlot.GetAmount());
        }
        PickUpObjList.instance.DestroyOnList(gameObject.name);
        Destroy(gameObject);
        return;
    }

    public Vector3 GetNearPoint()
    {
        if (nearPoint == null)
            return transform.position;
        return nearPoint.position;
    }

    public GameObject GetMainGameObject()
    {
        return gameObject;
    }

    public bool CanUse()
    {
        return true;
    }
}
