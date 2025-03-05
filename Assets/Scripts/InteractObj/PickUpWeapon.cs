using UnityEngine;

public class PickUpWeapon : MonoBehaviour, IUsableObj
{
    [SerializeField] private Item item;
    [SerializeField] private SlotItem ammoSlot;
    [SerializeField] private Transform nearPoint;
    [SerializeField] private int idPickUp;
    private OutlineList outlines;
    private void Start()
    {
        bool result = PickUpObjList.instance.ExistOnList(idPickUp);
        if (result)
            Destroy(gameObject);
        outlines = GetComponent<OutlineList>();
        HideOutline();
    }
    public void Use()
    {
        Inventory.instance.AddNonStackableItem(item);
        if (ammoSlot != null && !ammoSlot.IsEmpty())
        {
            Inventory.instance.AddItem(ammoSlot.GetItem(), ammoSlot.GetAmount());
        }
        //PickUpObjList.instance.DestroyOnList(gameObject.name);
        PickUpObjList.instance.AddIdToList(idPickUp);
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

    public void ShowOutline()
    {
        if (outlines == null)
            return;
        outlines.Show(true);
    }

    public void HideOutline()
    {
        if (outlines == null)
            return;
        outlines.Show(false);
    }
}
