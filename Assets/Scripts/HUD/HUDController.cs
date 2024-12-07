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
    [SerializeField] private Button questPanelButton;
    [SerializeField] private Button slotButton;
    [SerializeField] private ButtonRightClick slotChangeStateButton;
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;
    [SerializeField] private Button skipButton;
    [Header("Console")]
    [SerializeField] private TextMeshProUGUI consoleText;
    [SerializeField] private List<string> consoleLogs = new List<string>();
    [SerializeField] private RectTransform upButtonConsoleTransform;
    [SerializeField] private RectTransform downButtonConsoleTransform;
    [SerializeField] private Button upConsoleButton;
    [SerializeField] private Button downConsoleButton;
    [Header("Slot Panel")]
    [SerializeField] private SlotState slotState = SlotState.Use;
    [SerializeField] private TextMeshProUGUI slotStateText;
    [SerializeField] private TextMeshProUGUI slotAmountText;
    [SerializeField] private Image slotItemImage;
    [SerializeField] private int currentSlotIndex = 0;
    [SerializeField] private int indexConsoleLog = 0;
    [Header("Timer")]
    [SerializeField] private TextMeshProUGUI timerText;
    [Header("Fight Panel")]
    [SerializeField] private GameObject fightPanel;
    private PlayerController player;
    private Canvas canvas;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
        canvas = GetComponent<Canvas>();
        showButton.onClick.AddListener(Show);
        hideButton.onClick.AddListener(Hide);
        slotButton.onClick.AddListener(OnClickSlot);
        leftButton.onClick.AddListener(OnClickLeftButton);
        rightButton.onClick.AddListener(OnClickRigtButton);
        slotChangeStateButton.OnClickRight.AddListener(ChangeStateSlot);
        pauseMenuButton.onClick.AddListener(OpenPauseMenu);
        inventoryButton.onClick.AddListener(OpenInventory);
        questPanelButton.onClick.AddListener(OpenQuestList);
        upConsoleButton.onClick.AddListener(OnClickButtonUpConsole);
        downConsoleButton.onClick.AddListener(OnClickButtonDownConsole);
        if(skipButton != null)
            skipButton.onClick.AddListener(OnClickSkip);
        if(fightPanel != null)
            fightPanel.SetActive(false);
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
        if(currentSlotIndex == Inventory.instance.GetLengthSlotItem())
            currentSlotIndex = 0;
        SetItemSlot();
    }
    
    public void OnClickLeftButton()
    {
        currentSlotIndex--;
        if(currentSlotIndex < 0)
            currentSlotIndex = Inventory.instance.GetLengthSlotItem() - 1;
        SetItemSlot();
    }



    public void OpenInventory()
    {
        InventoryUI.instance.Show();
        player.StopMove();
    }

    public void OpenPauseMenu()
    {
        PauseMenuDemo.instance.Show();
    }

    public void OpenQuestList()
    {
        QuestListUI.instance.Show();
    }

    public void SetActiveInventoryBtn(bool _active)
    {
        inventoryButton.enabled = _active;
    }

    public void SetActiveCanvas(bool value)
    {
        canvas.enabled = value;
    }

    public SlotItem GetCurrentItem()
    {
        return Inventory.instance.GetSlotItem(currentSlotIndex);
    }

    public void RemoveCurrentItem()
    {
        SlotItem slotItem = GetCurrentItem();
        if (slotItem.GetAmount() > 1)
        {
            Inventory.instance.AddAmountToSlot(currentSlotIndex, -1);
            SetItemSlot();
            return;
        }
        Inventory.instance.SetAmountSlot(currentSlotIndex, 0);
        Inventory.instance.SetNullSlot(currentSlotIndex);
        SetItemSlot();
    }

    public bool AddItemToSlot(Item item)
    {
        int freeIndex = 0;
        foreach (SlotItem slot in Inventory.instance.GetSlots())
        {
            if(slot.IsEmpty())
            {
                break;
            }
            freeIndex++;
        }
        if (freeIndex == Inventory.instance.GetLengthSlotItem())
            return false;
        Inventory.instance.SetSlot(currentSlotIndex, new SlotItem(item, 1));
        if(freeIndex == currentSlotIndex)
            SetItemSlot();
        return true;
    }

    public void AddItemToSlot(SlotItem item, int _index)
    {
        Inventory.instance.SetSlot(_index, item);
        if (_index == currentSlotIndex)
            SetItemSlot();
    }

    public bool AddItemToHUDSlot(Item item, int amount = 1)
    {
        for (int i = 0; i < Inventory.instance.GetLengthSlotItem(); i++)
        {
            if (Inventory.instance.GetSlotItem(i) == null)
                continue;
            if (Inventory.instance.GetSlotItem(i).IsEmpty())
                continue;
            if (Inventory.instance.GetSlotItem(i).GetItem() == item)
            {
                Inventory.instance.AddAmountToSlot(i, amount);
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

    public bool PointerOnUpButtonConsole()
    {
        return RectTransformUtility.RectangleContainsScreenPoint(upButtonConsoleTransform, Input.mousePosition);
    }

    public bool PointerOnDownButtonConsole()
    {
        return RectTransformUtility.RectangleContainsScreenPoint(downButtonConsoleTransform, Input.mousePosition);
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
        {
            displayedLogs = consoleLogs.GetRange(consoleLogs.Count - 7, 7);
            indexConsoleLog = consoleLogs.Count - 7;
        }
        else
        {
            displayedLogs = new List<string>(consoleLogs);
        }
        consoleText.text = string.Empty;
        foreach(string newLog in displayedLogs)
        {
            consoleText.text += $"{newLog}\n";
        }
    }

    public void OnClickButtonUpConsole()
    {
        if (consoleLogs.Count <= 7)
            return;
        indexConsoleLog -= 1;
        if (indexConsoleLog < 0)
            indexConsoleLog = 0;
        List<string> displayedLogs = consoleLogs.GetRange(indexConsoleLog, 7);
        consoleText.text = string.Empty;
        foreach (string newLog in displayedLogs)
        {
            consoleText.text += $"{newLog}\n";
        }
    }

    public void OnClickButtonDownConsole()
    {
        if (consoleLogs.Count <= 7)
            return;
        indexConsoleLog += 1;
        if(indexConsoleLog > consoleLogs.Count - 7)
            indexConsoleLog = consoleLogs.Count - 7;
        List<string> displayedLogs = consoleLogs.GetRange(indexConsoleLog, 7);
        consoleText.text = string.Empty;
        foreach (string newLog in displayedLogs)
        {
            consoleText.text += $"{newLog}\n";
        }
    }

    public void OnClickSkip()
    {
        if (GameParam.instance.inCombat)
            CombatController.instance.SkipTurnPlayer();
    }

    public void ShowFightPanel()
    {
        if(fightPanel != null)
            fightPanel.SetActive(true);
    }

    public void HideFightPanel()
    {
        if (fightPanel != null)
            fightPanel.SetActive(false);
    }

    private IEnumerator UpdateTimer()
    {
        while (true)
        {
            string time = TimeGame.instance.GetCurrentTimeString();
            timerText.text = time;
            yield return new WaitForEndOfFrame();
        }
    }
}

public enum SlotState
{
    Use, Reload, None
}
