using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class BulletMachine : MonoBehaviour, IUsableObj
{
    public Transform nearPoint;
    [SerializeField] private SlotItem money;
    [SerializeField] private List<SlotItem> items = new List<SlotItem>();
    private void Start()
    {
        
    }
    public bool CanUse()
    {
        return true;
    }

    public GameObject GetMainGameObject()
    {
        return gameObject;
    }

    public Vector3 GetNearPoint()
    {
        return nearPoint.position;
    }

    public void Use()
    {
        if (!Inventory.instance.PlayerHaveItem(money))
        {
            HUDController.instance.AddConsolelog("You don’t have enough");
            HUDController.instance.AddConsolelog("money.");
            return;
        }
        int randomItem = Random.Range(0, items.Count);
        Inventory.instance.AddItem(items[randomItem]);
        MiscItem miscItem = (MiscItem)items[randomItem].GetItem();
        HUDController.instance.AddConsolelog("You got 1 random round");
        if (miscItem.isAmmo)
        {
            WeaponObject weapon = WeaponController.instance.GetWeaponByAmmo(items[randomItem].GetItem().id);
            if (weapon != null)
                weapon.UpdateAmmoSlot();
        }
        Inventory.instance.RemoveItem(money);
    }

}
