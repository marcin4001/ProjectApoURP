using UnityEngine;
using UnityEngine.EventSystems;

public class SlotDropUI : MonoBehaviour, IDropHandler
{
    [SerializeField] private int slotIndex = 0;
    [SerializeField] private Vector2 positionItem;
    [SerializeField] private SlotItemUI slotItemUI;
    [SerializeField] private bool armorSlot = false;
    void Start()
    {
        
    }
    public void OnDrop(PointerEventData eventData)
    {
        
        if(eventData.pointerDrag != null)
        {
            if(armorSlot)
            {
                if (slotItemUI != null)
                    return;
                slotItemUI = eventData.pointerDrag.GetComponent<SlotItemUI>();
                if (slotItemUI != null)
                {
                    SlotItem slot = slotItemUI.GetSlot();
                    if (slot.GetItem() is ArmorItem)
                    {
                        slotItemUI.SetSlotDrop(this);
                        Inventory.instance.RemoveItem(slot);
                        HUDController.instance.AddItemToSlot(slot, -1);
                    }
                    else
                    {
                        slotItemUI = null;
                    }
                }
                return;
            }
            if (slotItemUI != null)
            {
                SlotItem slot = slotItemUI.GetSlot();
                if (slot.GetItem() is WeaponItem)
                    return;
                if (slot.GetItem() is ArmorItem)
                    return;
                SlotItemUI slotItemUITemp = eventData.pointerDrag.GetComponent<SlotItemUI>();
                if(slotItemUITemp != null)
                {
                    SlotItem slot2 = slotItemUITemp.GetSlot();
                    if(slot.GetItem() == slot2.GetItem())
                    {
                        int newAmount = slot.GetAmount() + slot2.GetAmount();
                        slot.SetAmount(newAmount);
                        slotItemUI.UpdateAmountText();
                        Inventory.instance.RemoveItem(slot2);
                        HUDController.instance.UpdateCurrentSlotAmountText();
                        Destroy(slotItemUITemp.gameObject);
                    }
                }
                return;
            }
            slotItemUI = eventData.pointerDrag.GetComponent<SlotItemUI>();
            if(slotItemUI != null)
            {
                SlotItem slot = slotItemUI.GetSlot();
                if (slot.GetItem() is MiscItem)
                {
                    MiscItem miscItem = (MiscItem)slot.GetItem();
                    if (miscItem.isAmmo)
                    {
                        slotItemUI = null;
                        return;
                    }
                }
                slotItemUI.SetSlotDrop(this);
                Inventory.instance.RemoveItem(slot);
                HUDController.instance.AddItemToSlot(slot, slotIndex);
            }
        }
    }

    public Vector2 GetPositionItem()
    {
        return positionItem;
    }

    public void SetEmpty()
    {
        slotItemUI = null;
        if(armorSlot)
        {
            HUDController.instance.AddItemToSlot(new SlotItem(null, 0), -1);
            return;
        }

        HUDController.instance.AddItemToSlot(new SlotItem(null, 0), slotIndex);
    }

    private void DestroySlotItemUI()
    {
        if(slotItemUI != null)
        {
            Destroy(slotItemUI.gameObject);
        }
    }

    public void UpdateAmountText()
    {
        if (slotItemUI != null)
        {
            if(slotItemUI.isEmpty())
            {
                DestroySlotItemUI();
                return;
            }
            slotItemUI.UpdateAmountText();
        }
    }

    public void AddSlot(SlotItemUI slotItem)
    {
        slotItemUI = slotItem;
        slotItemUI.SetSlotDrop(this);
        slotItemUI.SetInDropSlot();
    }
}
