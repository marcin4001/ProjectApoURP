using UnityEngine;
using UnityEngine.EventSystems;

public class SlotDropUI : MonoBehaviour, IDropHandler
{
    [SerializeField] private int slotIndex = 0;
    [SerializeField] private Vector2 positionItem;
    [SerializeField] private SlotItemUI slotItemUI;
    void Start()
    {
        
    }
    public void OnDrop(PointerEventData eventData)
    {
        if (slotItemUI != null)
            return;
        if(eventData.pointerDrag != null)
        {
            slotItemUI = eventData.pointerDrag.GetComponent<SlotItemUI>();
            if(slotItemUI != null)
            {
                slotItemUI.SetSlotDrop(this);
                SlotItem slot = slotItemUI.GetSlot();
                Inventory.instance.RemoveItem(slot);
                HUDController.instance.AddItemToSlot(slot.GetItem(), slotIndex);
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
        HUDController.instance.AddItemToSlot(null, slotIndex);
    }

    public void DestroySlotItemUI()
    {
        if(slotItemUI != null)
        {
            Destroy(slotItemUI.gameObject);
        }
    }
}
