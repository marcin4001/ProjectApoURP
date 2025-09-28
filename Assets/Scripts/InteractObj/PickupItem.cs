using UnityEngine;

public class PickupItem : MonoBehaviour, IUsableObj
{
    [SerializeField] private Item item;
    [SerializeField] private int amount = 1;
    [SerializeField] private Transform nearPoint;
    [SerializeField] private int idPickUp;
    [SerializeField] private bool instantReadBook = false;
    [SerializeField] private BookProfile book;
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
        //PickUpObjList.instance.DestroyOnList(gameObject.name);
        if (instantReadBook)
        {
            BookReader.instance.Show(book);
            book.Execute();
        }
        PickUpObjList.instance.AddIdToList(idPickUp);
        Destroy(gameObject);
    }

    public Vector3 GetNearPoint()
    {
        if(nearPoint == null)
            return transform.position;
        return nearPoint.position;
    }

    public bool CanUse()
    {
        return true;
    }

    public GameObject GetMainGameObject()
    {
        return gameObject;
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
