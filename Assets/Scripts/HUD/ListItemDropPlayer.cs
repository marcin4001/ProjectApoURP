using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;

public class ListItemDropPlayer : MonoBehaviour, IDropHandler
{
    void Start()
    {
        
    }
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            SlotItemUI slotItemUI = eventData.pointerDrag.GetComponent<SlotItemUI>();
            if (slotItemUI == null)
                return;
            if (slotItemUI.IsPlayerItem())
                return;
            SlotItem slotItem = slotItemUI.GetSlot();
            if (slotItem == null || slotItem.IsEmpty())
                return;
            Cabinet cabinet = CabinetUI.instance.GetCabinet();
            if (cabinet != null)
            {
                cabinet.RemoveItem(slotItem);
                Inventory.instance.AddItem(slotItem);
            }
            Item item = slotItem.GetItem();
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

            Destroy(slotItemUI.gameObject);
            CabinetUI.instance.ReCreateList();
        }
    }
}
