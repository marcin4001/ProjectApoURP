using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI instance;
    [SerializeField] private Button closeButton;
    [SerializeField] private TextMeshProUGUI consoleText;
    [SerializeField] private Transform content;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private SlotDropUI[] slotsDrop;
    [SerializeField] private bool active = false;
    [SerializeField] private string separator = "-------------------------------";
    [SerializeField] private ScrollListController scrollListItems;
    private Canvas canvas;
    private PlayerController player;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        canvas = GetComponent<Canvas>();
        player = FindFirstObjectByType<PlayerController>();
        closeButton.onClick.AddListener(Hide);
        canvas.enabled = false;
    }

    public bool GetActive()
    {
        return active;
    }

    public void Show()
    {
        canvas.enabled = true;
        player.SetInMenu(true);
        CreateListItem();
        UpdateAmountTextSlotsDrop();
        if(scrollListItems != null)
            scrollListItems.ResetPositionList();
        consoleText.text = string.Empty;
        active = true;
        CameraMovement.instance.SetBlock(true);
    }

    public void Hide()
    {
        canvas.enabled = false;
        player.SetInMenu(false);
        active = false;
        CameraMovement.instance.SetBlock(false);
    }

    public Canvas GetCanvas()
    {
        return canvas;
    }

    public void CreateListItem()
    {
        foreach(Transform slot in content.transform)
        {
            Destroy(slot.gameObject);
        }
        List<SlotItem> slotItems = Inventory.instance.GetItems();
        foreach(SlotItem item in slotItems)
        {
            SlotItemUI slot = Instantiate(slotPrefab, content).GetComponent<SlotItemUI>();
            slot.SetSlot(item);
        }
    }


    public void UpdateAmountTextSlotsDrop()
    {
        for(int i = 0; i < slotsDrop.Length; i++)
            slotsDrop[i].UpdateAmountText();
    }


    public void ShowDescription(SlotItem slot)
    {
        Item item = slot.GetItem();
        consoleText.text = $"{item.nameItem}\n{separator}\n{item.description}";
        if (slot.GetAmount() > 1)
            consoleText.text += $"\nAmount: {slot.GetAmount()}";
        consoleText.text += $"\nValue: ${slot.GetItem().value}";
    }
}
