using System.Collections.Generic;
using UnityEngine;

public class AddAllWeapons : MonoBehaviour
{
    void Update()
    {
        if (!GameParam.instance.inDev)
            return;
        if (Input.GetKeyDown(KeyCode.F4))
        {
            List<Item> listWeapons = ItemDB.instance.GetWeaponItems();
            foreach(Item weapon in  listWeapons)
            {
                SlotItem slotItem = new SlotItem(weapon, 1);
                Inventory.instance.AddItem(slotItem);
            }
            Destroy(gameObject);
        }
    }
}
