using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;

public class ListItemDropPlayer : MonoBehaviour, IDropHandler
{
    [SerializeField] private SlotItem slot;

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
            if (slotItem.GetAmount() == 1)
            {
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
            else
            {
                slot = slotItem;
                CounterUI.instance.Show(slotItem.GetAmount());
                StartCoroutine(UseCounter());
            }
        }
    }

    private IEnumerator UseCounter()
    {
        while(!CounterUI.instance.IsApply())
        {
            yield return null;
        }

        Cabinet cabinet = CabinetUI.instance.GetCabinet();
        int newAmount = CounterUI.instance.GetNumber();
        SlotItem newItem = new SlotItem(slot.GetItem(), newAmount);
        Inventory.instance.AddItem(newItem);
        cabinet.RemoveItem(newItem);
        Item item = slot.GetItem();
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
        CabinetUI.instance.ReCreateList();
    }
}
