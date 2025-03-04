using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ListItemDropCabinet : MonoBehaviour, IDropHandler
{
    [SerializeField] private SlotItem slot;
    void Start()
    {
        
    }
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            if(CabinetUI.instance.GetCabinet() == null)
                return;
            SlotItemUI slotItemUI = eventData.pointerDrag.GetComponent<SlotItemUI>();
            if (slotItemUI == null)
                return;
            if (!slotItemUI.IsPlayerItem())
                return;
            SlotItem slotItem = slotItemUI.GetSlot();
            if (slotItem == null || slotItem.IsEmpty())
                return;
            if (slotItem.GetAmount() == 1)
            {
                Cabinet cabinet = CabinetUI.instance.GetCabinet();
                if (cabinet != null)
                {
                    cabinet.AddItem(slotItem);
                    Inventory.instance.RemoveItem(slotItem);
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
                            weapon.UpdateAmmoSlot();
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
        while (!CounterUI.instance.IsApply())
        {
            yield return null;
        }

        Cabinet cabinet = CabinetUI.instance.GetCabinet();
        int newAmount = CounterUI.instance.GetNumber();
        SlotItem newItem = new SlotItem(slot.GetItem(), newAmount);
        Inventory.instance.RemoveItem(newItem);
        cabinet.AddItem(newItem);
        Item item = slot.GetItem();
        if (item is MiscItem)
        {
            MiscItem miscItem = (MiscItem)item;
            if (miscItem.isAmmo)
            {
                Debug.Log("IsAmmo");
                WeaponObject weapon = WeaponController.instance.GetWeaponByAmmo(item.id);
                if (weapon != null)
                {
                    weapon.UpdateAmmoSlot();
                    Debug.Log("UpdateAmmoSlot");
                }
            }
        }
        CabinetUI.instance.ReCreateList();
    }
}
