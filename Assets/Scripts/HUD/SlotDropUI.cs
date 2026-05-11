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
                {
                    SlotItemUI slotItemUITempA = eventData.pointerDrag.GetComponent<SlotItemUI>();
                    SlotItem slotA = slotItemUITempA.GetSlot();
                    if (slotA.GetItem() is ArmorItem)
                    {
                        SlotItem slot = slotItemUI.GetSlot();
                        Inventory.instance.AddItem(slot);
                        Destroy(slotItemUI.gameObject);
                        slotItemUI = slotItemUITempA;
                        slotItemUI.SetSlotDrop(this);
                        Inventory.instance.RemoveItem(slotA);
                        HUDController.instance.AddItemToSlot(slotA, -1);
                        ArmorItem armor = (ArmorItem)slotA.GetItem();
                        PlayerClothes.instance.SetClothes(armor);
                        QuestListUI.instance.SetArmorItem(armor);
                        StatsPanelNewLevel.instance.SetArmorItem(armor);
                        InventoryUI.instance.CreateListItem();
                    }
                    return;
                }
                slotItemUI = eventData.pointerDrag.GetComponent<SlotItemUI>();
                if (slotItemUI != null)
                {
                    SlotItem slot = slotItemUI.GetSlot();
                    if (slot.GetItem() is ArmorItem)
                    {
                        slotItemUI.SetSlotDrop(this);
                        Inventory.instance.RemoveItem(slot);
                        HUDController.instance.AddItemToSlot(slot, -1);
                        ArmorItem armor = (ArmorItem) slot.GetItem();
                        PlayerClothes.instance.SetClothes(armor);
                        QuestListUI.instance.SetArmorItem(armor);
                        StatsPanelNewLevel.instance.SetArmorItem(armor);
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

                SlotItemUI slotItemUITemp = eventData.pointerDrag.GetComponent<SlotItemUI>();
                if(slotItemUITemp != null)
                {
                    SlotItem slot2 = slotItemUITemp.GetSlot();
                    if(slot.GetItem() == slot2.GetItem())
                    {
                        if (slotItemUI == slotItemUITemp)
                            return;
                        if (slot.GetItem() is WeaponItem)
                            return;
                        if (slot.GetItem() is ArmorItem)
                            return;
                        int newAmount = slot.GetAmount() + slot2.GetAmount();
                        slot.SetAmount(newAmount);
                        slotItemUI.UpdateAmountText();
                        Inventory.instance.RemoveItem(slot2);
                        HUDController.instance.UpdateCurrentSlotAmountText();
                        Destroy(slotItemUITemp.gameObject);
                    }
                    else
                    {
                        Debug.Log("item1 != item2");
                        if (slot2.GetItem() is MiscItem)
                        {
                            MiscItem miscItem = (MiscItem)slot2.GetItem();
                            if (miscItem.isAmmo)
                                return;
                        }
                        if (slot2.GetItem() is WeaponItem)
                        {
                            WeaponItem weaponItem = (WeaponItem)slot2.GetItem();
                            int strength = PlayerStats.instance.GetStrength();
                            if (weaponItem.strengthRequired > strength)
                            {
                                HUDController.instance.AddConsolelog("You don’t have enough");
                                HUDController.instance.AddConsolelog("Strength to use this");
                                HUDController.instance.AddConsolelog("weapon!");
                                return;
                            }
                        }
                        bool haveDropSlot = slotItemUITemp.HaveSlotDrop();
                        Inventory.instance.AddItem(slot);
                        Destroy(slotItemUI.gameObject);
                        slotItemUI = slotItemUITemp;
                        slotItemUI.SetSlotDrop(this);
                        if (!haveDropSlot)
                            Inventory.instance.RemoveItem(slot2);
                        HUDController.instance.AddItemToSlot(slot2, slotIndex);
                        InventoryUI.instance.CreateListItem();
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
                if(slot.GetItem() is WeaponItem)
                {
                    WeaponItem weaponItem = (WeaponItem)slot.GetItem();
                    int strength = PlayerStats.instance.GetStrength();
                    if(weaponItem.strengthRequired >  strength)
                    {
                        slotItemUI = null;
                        HUDController.instance.AddConsolelog("You don’t have enough");
                        HUDController.instance.AddConsolelog("Strength to use this");
                        HUDController.instance.AddConsolelog("weapon!");
                        return;
                    }
                }
                bool haveDropSlot = slotItemUI.HaveSlotDrop();
                slotItemUI.SetSlotDrop(this);
                if(!haveDropSlot)
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
            PlayerClothes.instance.ResetClothes();
            QuestListUI.instance.ClearArmorItem();
            StatsPanelNewLevel.instance.ClearArmorItem();
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
        if (armorSlot)
        {
            SlotItem slot = slotItemUI.GetSlot();
            if (slot.GetItem() is ArmorItem)
            {
                ArmorItem armor = (ArmorItem)slot.GetItem();
                PlayerClothes.instance.SetClothes(armor);
                QuestListUI.instance.SetArmorItem(armor);
                StatsPanelNewLevel.instance.SetArmorItem(armor);
            }
        }
    }
}
