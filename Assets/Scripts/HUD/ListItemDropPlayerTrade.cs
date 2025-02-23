using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ListItemDropPlayerTrade : MonoBehaviour, IDropHandler
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
                if(slotItem.GetItem().value > TradeUI.instance.GetMoneyPlayer())
                {
                    TradeUI.instance.ShowInfoPlayerHaveEnoughMoney();
                    return;
                }
                DialogueNPC nPC = TradeUI.instance.GetNPC();
                SlotItem money = TradeUI.instance.GetNewMoneySlot(slotItem.GetItem().value);
                if (nPC != null)
                {
                    nPC.RemoveItem(slotItem);
                    nPC.AddItem(money);
                    Inventory.instance.AddItem(slotItem);
                    Inventory.instance.RemoveItem(money);
                    
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
                            weapon.UpdateAmmoSlot();//weapon.UpdateAmmoOutGun();
                        }
                    }
                }

                Destroy(slotItemUI.gameObject);
                TradeUI.instance.ReCreateList();
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

        DialogueNPC nPC = TradeUI.instance.GetNPC();
        int newAmount = CounterUI.instance.GetNumber();
        int value = newAmount * slot.GetItem().value;
        if (value <= TradeUI.instance.GetMoneyPlayer())
        {
            SlotItem money = TradeUI.instance.GetNewMoneySlot(value);
            SlotItem newItem = new SlotItem(slot.GetItem(), newAmount);
            Inventory.instance.AddItem(newItem);
            Inventory.instance.RemoveItem(money);
            if (nPC != null)
            {
                nPC.RemoveItem(newItem);
                nPC.AddItem(money);
            }
            Item item = slot.GetItem();
            if (item is MiscItem)
            {
                MiscItem miscItem = (MiscItem)item;
                if (miscItem.isAmmo)
                {
                    WeaponObject weapon = WeaponController.instance.GetWeaponByAmmo(item.id);
                    if (weapon != null)
                    {
                        weapon.UpdateAmmoSlot();//weapon.UpdateAmmoOutGun();
                    }
                }
            }
            TradeUI.instance.ReCreateList();
        }
        else
        {
            TradeUI.instance.ShowInfoPlayerHaveEnoughMoney();
            slot = null;
        }
        
    }
}
