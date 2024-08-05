using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotItemUI : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private SlotItem slot;
    [SerializeField] private Vector2 sizeOnList;
    [SerializeField] private Vector2 sizeOnSlot;
    [SerializeField] private float sizeFontOnList = 20;
    [SerializeField] private float sizeFontOnSlot = 30;
    [SerializeField] private SlotDropUI slotDrop;
    private Image image;
    private Transform parent;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Canvas canvas;
    private Button button;
    private TextMeshProUGUI amountText;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = InventoryUI.instance.GetCanvas();
        canvasGroup = GetComponent<CanvasGroup>();
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
        amountText = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void OnClick()
    {
        if(slot != null)
        {
            InventoryUI.instance.ShowDescription(slot);
        }
    }

    public SlotItem GetSlot()
    {
        return slot;
    }

    public void SetSlot(SlotItem _slot)
    {
        slot = _slot;
        if (image == null)
            image = GetComponent<Image>();
        if(amountText == null)
            amountText = GetComponentInChildren<TextMeshProUGUI>();
        image.overrideSprite = slot.GetItem().uiSprite;
        if (slot.GetAmount() > 1)
            amountText.text = $"x{slot.GetAmount()}";
        else
            amountText.text = string.Empty;
    }

    public void UpdateAmountText()
    {
        if (slot.GetAmount() > 1)
            amountText.text = $"x{slot.GetAmount()}";
        else
            amountText.text = string.Empty;
    }


    public void SetParent(Transform newParent)
    {
        parent = newParent;
    }

    public bool HaveSlotDrop()
    {
        return slotDrop != null;
    }

    public void SetSlotDrop(SlotDropUI _slotDrop)
    {
        if (slotDrop != null)
            slotDrop.SetEmpty();
        slotDrop = _slotDrop;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (slot != null)
        {
            InventoryUI.instance.ShowDescription(slot);
        }
        if (slotDrop ==  null)
            parent = rectTransform.parent;
        rectTransform.SetParent(canvas.transform);
        rectTransform.sizeDelta = sizeOnSlot;
        canvasGroup.blocksRaycasts = false;
        amountText.text = string.Empty;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (slot.GetAmount() > 1)
            amountText.text = $"x{slot.GetAmount()}";
        if (slotDrop != null)
        {
            RectTransform slotDropTransform  = slotDrop.GetComponent<RectTransform>();
            rectTransform.SetParent(slotDropTransform);
            rectTransform.sizeDelta = sizeOnSlot;
            amountText.fontSize = sizeFontOnSlot;
            rectTransform.anchoredPosition = slotDrop.GetPositionItem();
            canvasGroup.blocksRaycasts = true;
            return;
        }
        rectTransform.SetParent(parent);
        rectTransform.sizeDelta = sizeOnList;
        amountText.fontSize = sizeFontOnList;
        canvasGroup.blocksRaycasts = true;    
    }

}
