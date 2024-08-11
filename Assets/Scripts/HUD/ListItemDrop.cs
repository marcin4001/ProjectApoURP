using UnityEngine;
using UnityEngine.EventSystems;

public class ListItemDrop : MonoBehaviour, IDropHandler
{
    void Start()
    {
        
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            SlotItemUI slotItem = eventData.pointerDrag.GetComponent<SlotItemUI>();
            if (slotItem != null)
            {
                if (!slotItem.HaveSlotDrop())
                    return;
                slotItem.SetSlotDrop(null);
                SlotItem slot = slotItem.GetSlot();
                Inventory.instance.AddItem(slot);
                Destroy(slotItem.gameObject);
                InventoryUI.instance.CreateListItem();
            }
        }
    }
}
