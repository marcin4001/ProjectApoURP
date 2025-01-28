using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ListItemDropNPCTrade : MonoBehaviour, IDropHandler
{
    [SerializeField] private SlotItem slot;
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
            if (!slotItemUI.IsPlayerItem())
                return;
            SlotItem slotItem = slotItemUI.GetSlot();
            if (slotItem == null || slotItem.IsEmpty())
                return;
            if (slotItem.GetAmount() == 1)
            {
                if(slotItem.GetItem().value > TradeUI.instance.GetMoneyNPC())
                {
                    TradeUI.instance.ShowInfoNPCHaveEnoughMoney();
                    return;
                }
                SlotItem money = TradeUI.instance.GetNewMoneySlot(slotItem.GetItem().value);
                DialogueNPC npc = TradeUI.instance.GetNPC();  
                if (npc != null)
                {
                    npc.AddItem(slotItem);
                    npc.RemoveItem(money);
                    Inventory.instance.RemoveItem(slotItem);
                    Inventory.instance.AddItem(money);
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

        DialogueNPC npc = TradeUI.instance.GetNPC();
        int newAmount = CounterUI.instance.GetNumber();
        int value = slot.GetItem().value * newAmount;
        if (value <= TradeUI.instance.GetMoneyNPC())
        {
            SlotItem money = TradeUI.instance.GetNewMoneySlot(value);
            SlotItem newItem = new SlotItem(slot.GetItem(), newAmount);
            Inventory.instance.AddItem(money);
            Inventory.instance.RemoveItem(newItem);
            npc.RemoveItem(money);
            npc.AddItem(newItem);
            Item item = slot.GetItem();
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
            TradeUI.instance.ReCreateList();
        }
        else
        {
            TradeUI.instance.ShowInfoNPCHaveEnoughMoney();
            slot = null;
        }
    }
}
