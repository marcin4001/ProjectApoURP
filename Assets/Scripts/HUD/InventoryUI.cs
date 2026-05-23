using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI instance;
    [SerializeField] private Button closeButton;
    [SerializeField] private TextMeshProUGUI consoleText;
    [SerializeField] private Transform content;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private SlotDropUI[] slotsDrop;
    [SerializeField] private SlotDropUI armorSlotDrop;
    [SerializeField] private bool active = false;
    [SerializeField] private string separator = "-------------------------------";
    [SerializeField] private ScrollListController scrollListItems;
    private MainInputSystem inputSystem;
    private Canvas canvas;
    private PlayerController player;
    private void Awake()
    {
        instance = this;
        inputSystem = new MainInputSystem();
        inputSystem.Player.Inventory.performed += ShowByKey;
        inputSystem.Enable();
    }

    private void OnEnable()
    {
        inputSystem.Player.Inventory.performed += ShowByKey;
        inputSystem.Enable();
    }

    private void OnDisable()
    {
        inputSystem.Player.Inventory.performed -= ShowByKey;
        inputSystem.Disable();
    }

    void Start()
    {
        canvas = GetComponent<Canvas>();
        player = FindFirstObjectByType<PlayerController>();
        closeButton.onClick.AddListener(Hide);
        for(int i = 0; i < slotsDrop.Length; i++)
        {
            SlotItem item = Inventory.instance.GetSlotItem(i);
            if(item.IsEmpty())
                continue;
            SlotItemUI slot = Instantiate(slotPrefab, content).GetComponent<SlotItemUI>();
            slot.SetSlot(item);
            slotsDrop[i].AddSlot(slot);
        }
        if(armorSlotDrop != null)
        {
            SlotItem item = Inventory.instance.GetArmorItem();
            if(!item.IsEmpty())
            {
                SlotItemUI slot = Instantiate(slotPrefab, content).GetComponent<SlotItemUI>();
                slot.SetSlot(item);
                armorSlotDrop.AddSlot(slot);
            }
        }
        canvas.enabled = false;
    }

    public bool GetActive()
    {
        return active;
    }

    private void ShowByKey(InputAction.CallbackContext ctx)
    {
        if (CursorController.instance.IsWait())
            return;
        if (!active)
        {
            if(!player.InMenu()) Show();
        }
        else
        {
            Hide();
        }

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
        player.StopMove();
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
