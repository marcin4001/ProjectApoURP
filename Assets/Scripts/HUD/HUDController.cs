using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using System.Collections;

public class HUDController : MonoBehaviour
{
    public static HUDController instance;
    [Header("Background")]
    [SerializeField] private RectTransform background;
    [Header("State Buttons")]
    [SerializeField] private StateButton moveStateButton;
    [SerializeField] private StateButton useStateButton;
    [SerializeField] private StateButton lookStateButton;
    [Header("Bars")]
    [SerializeField] private Image hpBar;
    [SerializeField] private Image radBar;
    [Header("HUD Buttons")]
    [SerializeField] private Button hideButton;
    [SerializeField] private Button showButton;
    [SerializeField] private Button pauseMenuButton;
    [SerializeField] private Button inventoryButton;
    [SerializeField] private Button slotButton;
    [SerializeField] private ButtonRightClick slotChangeStateButton;
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;
    [Header("Console")]
    [SerializeField] private TextMeshProUGUI consoleText;
    [SerializeField] private List<string> consoleLogs = new List<string>();
    [Header("Slot Panel")]
    [SerializeField] private SlotState slotState = SlotState.Use;
    [SerializeField] private TextMeshProUGUI slotStateText;
    [SerializeField] private TextMeshProUGUI slotAmountText;
    [SerializeField] private Image slotItemImage;
    [SerializeField] private SlotItem[] slots;
    [SerializeField] private int currentSlotIndex = 0;
    [Header("Timer")]
    [SerializeField] private TextMeshProUGUI timerText;
    private PlayerController player;
    private Canvas canvas;
    private TimeGame gameTime;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
        gameTime = FindFirstObjectByType<TimeGame>();
        canvas = GetComponent<Canvas>();
        showButton.onClick.AddListener(Show);
        hideButton.onClick.AddListener(Hide);
        slotButton.onClick.AddListener(OnClickSlot);
        leftButton.onClick.AddListener(OnClickLeftButton);
        rightButton.onClick.AddListener(OnClickRigtButton);
        slotChangeStateButton.OnClickRight.AddListener(ChangeStateSlot);
        pauseMenuButton.onClick.AddListener(OpenPauseMenu);
        inventoryButton.onClick.AddListener(OpenInventory);
        consoleText.text = string.Empty;
        slotStateText.text = slotState.ToString();
        Show();
        SetItemSlot();
        StartCoroutine(UpdateTimer());
    }

    public void Show()
    {
        background.gameObject.SetActive(true);
        showButton.gameObject.SetActive(false);
    }

    public void Hide()
    {
        background.gameObject.SetActive(false);
        showButton.gameObject.SetActive(true);
    }

    public void OnClickSlot()
    {
        SlotItem slotItem = GetCurrentItem();
        Item item = slotItem.GetItem();
        if (item is WeaponItem)
        {
            switch (slotState)
            {
                case SlotState.Use:
                    player.StartUsingItem();
                    break;
                case SlotState.Reload:
                    player.ReloadGun();
                    break;
            }
        }
        if(item is FoodItem)
        {
            switch (slotState)
            {
                case SlotState.Use:
                    player.Eat(item);
                    break;
            }
        }
    }

    public void ChangeStateSlot()
    {
        SlotItem slotItem = GetCurrentItem();
        Item item = slotItem.GetItem();
        if (item is WeaponItem)
        {
            WeaponItem weaponItem = (WeaponItem)item;
            if (weaponItem.type == WeaponType.Melee)
                return;
            switch (slotState)
            {
                case SlotState.Use:
                    slotState = SlotState.Reload;
                    break;
                case SlotState.Reload:
                    slotState = SlotState.Use;
                    break;
            }
        slotStateText.text = slotState.ToString();
        }
    }

    public void OnClickRigtButton()
    {
        currentSlotIndex++;
        if(currentSlotIndex == slots.Length)
            currentSlotIndex = 0;
        SetItemSlot();
    }
    
    public void OnClickLeftButton()
    {
        currentSlotIndex--;
        if(currentSlotIndex < 0)
            currentSlotIndex = slots.Length - 1;
        SetItemSlot();
    }



    public void OpenInventory()
    {
        InventoryUI.instance.Show();
    }

    public void OpenPauseMenu()
    {
        PauseMenuDemo.instance.Show();
    }

    public void SetActiveCanvas(bool value)
    {
        canvas.enabled = value;
    }

    public SlotItem GetCurrentItem()
    {
        return slots[currentSlotIndex];
    }

    public void RemoveCurrentItem()
    {
        SlotItem slotItem = GetCurrentItem();
        if (slotItem.GetAmount() > 1)
        {
            int newAmount = slotItem.GetAmount() - 1;
            slotItem.SetAmount(newAmount);
            SetItemSlot();
            return;
        }
        slots[currentSlotIndex].SetAmount(0);
        slots[currentSlotIndex] = new SlotItem(null, 0);
        SetItemSlot();
    }

    public bool AddItemToSlot(Item item)
    {
        int freeIndex = 0;
        foreach (SlotItem slot in slots)
        {
            if(slot.IsEmpty())
            {
                break;
            }
            freeIndex++;
        }
        if (freeIndex == slots.Length)
            return false;
        slots[freeIndex] = new SlotItem(item, 1);
        if(freeIndex == currentSlotIndex)
            SetItemSlot();
        return true;
    }

    public void AddItemToSlot(SlotItem item, int _index)
    {
        slots[_index] = item;
        if (_index == currentSlotIndex)
            SetItemSlot();
    }

    public bool AddItemToHUDSlot(Item item, int amount = 1)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].IsEmpty())
                continue;
            if (slots[i].GetItem() == item)
            {
                int newAmount = slots[i].GetAmount() + amount;
                slots[i].SetAmount(newAmount);
                SetItemSlot();
                return true;
            }
        }
        return false;
    }

    public bool PointerOnHUD()
    {
        if (background.gameObject.activeSelf)
        {
            bool onBackground = RectTransformUtility.RectangleContainsScreenPoint(background, Input.mousePosition);
            bool onShowButton = RectTransformUtility.RectangleContainsScreenPoint(hideButton.GetComponent<RectTransform>(), Input.mousePosition);
            return onBackground || onShowButton;
        }
        return RectTransformUtility.RectangleContainsScreenPoint(showButton.GetComponent<RectTransform>(), Input.mousePosition);
    }

    public void SetStatePlayer(PlayerActionState state)
    {
        player.SetState(state);
        SetStateButtons(state);
    }

    

    public void SetStateButtons(PlayerActionState state)
    {
        UnselectAllStateButton();
        switch (state)
        {
            case PlayerActionState.Move:
                moveStateButton.ShowSelectFrame(); 
                break;
            case PlayerActionState.Use:
                useStateButton.ShowSelectFrame();
                break;
            case PlayerActionState.Look:
                lookStateButton.ShowSelectFrame();
                break;
        }
    }

    public SlotItem GetSlotById(int _id)
    {
        SlotItem slot = Array.Find(slots, x => x.GetItem().id == _id);
        if(slot != null)
            return slot;
        return new SlotItem(null, 0);
    }

    public void SetItemSlot()
    {
        SlotItem slotItem = GetCurrentItem();
        Item item = slotItem.GetItem();
        player.RemoveItemInHand();
        if(item == null)
        {
            slotState = SlotState.None;
            slotItemImage.enabled = false;
            player.ShowWeapon(null);
            slotAmountText.text = string.Empty;
        }
        else
        {
            slotState = SlotState.Use;
            slotItemImage.enabled = true;
            slotItemImage.overrideSprite = item.uiSprite;
            if (item is WeaponItem)
            {
                player.ShowWeapon(item);
            }
            else if (item is FoodItem)
            {
                player.ShowWeapon(null);
                player.SpawnItemInHand(item);
            }
            int amountItemSlot = slotItem.GetAmount();
            if (amountItemSlot > 1)
                slotAmountText.text = $"x{amountItemSlot}";
            else
                slotAmountText.text = string.Empty;
        }
        slotStateText.text = slotState.ToString();
    }

    public void UpdateCurrentSlotAmountText()
    {
        SlotItem slotItem = GetCurrentItem();
        if (slotItem == null || slotItem.IsEmpty())
            return;
        if(slotItem.GetAmount() > 1)
            slotAmountText.text = $"x{slotItem.GetAmount()}";
        else
            slotAmountText.text = string.Empty;
    }

    private void UnselectAllStateButton()
    {
        moveStateButton.HideSelectFrame();
        useStateButton.HideSelectFrame();
        lookStateButton.HideSelectFrame();
    }

    public void UpdateHPBar(int currentValue, int maxValue)
    {
        hpBar.fillAmount = (float)currentValue / (float)maxValue;
        if(hpBar.fillAmount < 0f) 
            hpBar.fillAmount = 0f;
        if(hpBar.fillAmount > 1f)
            hpBar.fillAmount = 1f;
    }

    public void UpdateRadBar(int currentValue, int maxValue)
    {
        radBar.fillAmount = (float)currentValue / (float)maxValue;
        if (radBar.fillAmount < 0f)
            radBar.fillAmount = 0f;
        if (radBar.fillAmount > 1f)
            radBar.fillAmount = 1f;
    }

    public void AddConsolelog(string log)
    {
        if (string.IsNullOrEmpty(log))
            return;
        consoleLogs.Add(log);
        List<string> displayedLogs;
        if (consoleLogs.Count > 7)
            displayedLogs = consoleLogs.GetRange(consoleLogs.Count - 7, 7);
        else
            displayedLogs = new List<string>(consoleLogs);
        consoleText.text = string.Empty;
        foreach(string newLog in displayedLogs)
        {
            consoleText.text += $"{newLog}\n";
        }
    }

    private IEnumerator UpdateTimer()
    {
        while (true)
        {
            string time = gameTime.GetCurrentTimeString();
            timerText.text = time;
            yield return new WaitForEndOfFrame();
        }
    }
}

public enum SlotState
{
    Use, Reload, None
}
