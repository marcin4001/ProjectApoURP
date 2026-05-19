using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using System.Collections;
using UnityEngine.InputSystem;

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
    [Header("Quest")]
    [SerializeField] private TextMeshProUGUI questText;
    [Header("Level")]
    [SerializeField] private Button newLevelBtn;
    [SerializeField] private Sprite newLevelNormal;
    [SerializeField] private Sprite newLevelActive;
    private PlayerController player;
    private Canvas canvas;
    private MainInputSystem inputSystem;

    private void Awake()
    {
        instance = this;
        inputSystem = new MainInputSystem();
        inputSystem.Player.Slot1.performed += Slot1Click;
        inputSystem.Player.Slot2.performed += Slot2Click;
        inputSystem.Player.Slot3.performed += Slot3Click;
        inputSystem.Enable();
    }

    private void OnEnable()
    {
        inputSystem.Player.Slot1.performed += Slot1Click;
        inputSystem.Player.Slot2.performed += Slot2Click;
        inputSystem.Player.Slot3.performed += Slot3Click;
        inputSystem.Enable();
    }

    private void OnDisable()
    {
        inputSystem.Player.Slot1.performed -= Slot1Click;
        inputSystem.Player.Slot2.performed -= Slot2Click;
        inputSystem.Player.Slot3.performed -= Slot3Click;
        inputSystem.Disable();
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
        if(newLevelBtn != null)
        {
            newLevelBtn.onClick.AddListener(OnNewLevelClick);
            if(GameParam.instance.isLevelUp)
                newLevelBtn.GetComponent<Image>().overrideSprite = newLevelActive;
            else
                newLevelBtn.GetComponent<Image>().overrideSprite = newLevelNormal;
        }
        consoleText.text = string.Empty;
        slotStateText.text = slotState.ToString();
        if(slotState == SlotState.None)
        {
            slotStateText.text = "Unarmed";
        }
        if(questText != null)
        {
            questText.text = "";
        }
        Show();
        SetItemSlot();
        StartCoroutine(UpdateTimer());
    }

    public void Show()
    {
        if (CursorController.instance.IsWait())
            return;
        background.gameObject.SetActive(true);
        showButton.gameObject.SetActive(false);
    }

    public void Hide()
    {
        if (CursorController.instance.IsWait())
            return;
        player.StopUsingItem();
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
                    player.SetMultishot(false);
                    player.StartUsingItem();
                    break;
                case SlotState.MultiShot:
                    player.SetMultishot(true);
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
        if(item is MiscItem)
        {
            MiscItem miscItem = (MiscItem) item;
            if(miscItem.isKey)
            {
                switch (slotState)
                {
                    case SlotState.Use:
                        player.StartUsingItem();
                        break;
                }
            }
            if(miscItem.isBook)
            {
                BookReader.instance.Show(miscItem.bookProfile);
                return;
            }
            if(miscItem.addPointAttribute)
            {
                switch(miscItem.attribute)
                {
                    case PlayerAttributes.strength:
                        PlayerStats.instance.AddStrengthPoint();
                        break;
                    case PlayerAttributes.dexterity:
                        PlayerStats.instance.AddDexterityPoint();
                        break;
                    case PlayerAttributes.technical:
                        PlayerStats.instance.AddTechnicalPoint();
                        break;
                    case PlayerAttributes.perception:
                        PlayerStats.instance.AddPerceptionPoint();
                        break;
                }
                RemoveCurrentItem();
            }
        }
        if(item == null)
        {
            player.StartUsingItem();
        }
    }

    public void ChangeStateSlot()
    {
        player.StopUsingItem();
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
                    if(weaponItem.multiShot)
                        slotState = SlotState.MultiShot;
                    else
                        slotState = SlotState.Reload;
                    break;
                case SlotState.MultiShot:
                    slotState = SlotState.Reload;
                    break;
                case SlotState.Reload:
                    slotState = SlotState.Use;
                    break;
            }
            slotStateText.text = slotState.ToString();
            if (slotState == SlotState.None)
            {
                slotStateText.text = "Unarmed";
            }
        }
    }

    public void OnClickRigtButton()
    {
        if (CursorController.instance.IsWait())
            return;
        player.StopUsingItem();
        currentSlotIndex++;
        if(currentSlotIndex == Inventory.instance.GetLengthSlotItem())
            currentSlotIndex = 0;
        SetItemSlot();
    }
    
    public void OnClickLeftButton()
    {
        if (CursorController.instance.IsWait())
            return;
        player.StopUsingItem();
        currentSlotIndex--;
        if(currentSlotIndex < 0)
            currentSlotIndex = Inventory.instance.GetLengthSlotItem() - 1;
        SetItemSlot();
    }

    private void Slot1Click(InputAction.CallbackContext ctx)
    {
        if (CursorController.instance.IsWait())
            return;
        player.StopUsingItem();
        currentSlotIndex = 0;
        SetItemSlot();
    }

    private void Slot2Click(InputAction.CallbackContext ctx)
    {
        if (CursorController.instance.IsWait())
            return;
        player.StopUsingItem();
        currentSlotIndex = 1;
        SetItemSlot();
    }

    private void Slot3Click(InputAction.CallbackContext ctx)
    {
        if (CursorController.instance.IsWait())
            return;
        player.StopUsingItem();
        currentSlotIndex = 2;
        SetItemSlot();
    }

    public void OpenInventory()
    {
        if (CursorController.instance.IsWait())
            return;
        player.StopUsingItem();
        InventoryUI.instance.Show();
        player.StopMove();
    }

    public void OpenPauseMenu()
    {
        player.StopUsingItem();
        PauseMenuDemo.instance.Show();
    }

    public void OpenQuestList()
    {
        if (CursorController.instance.IsWait())
            return;
        player.StopUsingItem();
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

    public bool IsWeaponCurrentItem()
    {
        if(Inventory.instance.GetSlotItem(currentSlotIndex) != null)
        {
            Item currentItem = Inventory.instance.GetSlotItem(currentSlotIndex).GetItem();
            if(currentItem != null)
                return currentItem is WeaponItem;
            else
                return true;
        }
        return true;
    }

    public bool IsMeleeCurrentItem()
    {
        if(Inventory.instance.GetSlotItem(currentSlotIndex) != null)
        {
            Item currentItem = Inventory.instance.GetSlotItem(currentSlotIndex).GetItem();
            if (currentItem != null)
                return false;
            else
                return true;
        }
        return true;
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
            bool onFightButton = RectTransformUtility.RectangleContainsScreenPoint(fightPanel.GetComponent<RectTransform>(), Input.mousePosition);
            if(GameParam.instance.inCombat)
                return onBackground || onShowButton || onFightButton;
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
        if (CursorController.instance.IsWait())
            return;
        player.StopUsingItem();
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

            int amountItemSlot = slotItem.GetAmount();
            if (amountItemSlot > 1)
                slotAmountText.text = $"x{amountItemSlot}";
            else
                slotAmountText.text = string.Empty;
            if (item is WeaponItem)
            {
                player.ShowWeapon(item);
            }
            else if (item is FoodItem)
            {
                player.ShowWeapon(null);
                player.SpawnItemInHand(item);
            }
            
        }
        slotStateText.text = slotState.ToString();
        if (slotState == SlotState.None)
        {
            slotStateText.text = "Unarmed";
        }
    }

    public void UpdateAmmoText(string _text)
    {
        slotAmountText.text = _text;
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

    public void AddConsolelogWarning(string log)
    {
        if (string.IsNullOrEmpty(log))
            return;
        string logNew = $"<color=#FFB000>{log}</color>";
        consoleLogs.Add(logNew);
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
        foreach (string newLog in displayedLogs)
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
        {
            player.StopUsingItem();
            CombatController.instance.SkipTurnPlayer();
        }
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

    public void OnNewLevelClick()
    {
        if (CursorController.instance.IsWait())
            return;
        player.StopUsingItem();
        if (GameParam.instance.isLevelUp)
            StatsPanelNewLevel.instance.Open();
    }

    public void ActiveNewLevelBtn()
    {
        newLevelBtn.GetComponent<Image>().overrideSprite = newLevelActive;
    }

    public void DeactiveNewLevelBtn()
    {
        newLevelBtn.GetComponent<Image>().overrideSprite = newLevelNormal;
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

    public void SetQuestCompleteText(Quest quest)
    {
        if(questText == null) 
            return;
        questText.text = "Quest Complete:\n";
        questText.text += quest.questTitle;
        StartCoroutine(HideQuestText());
    }

    private IEnumerator HideQuestText()
    {
        yield return new WaitForSeconds(5f);
        questText.text = "";
    }

    //void OnGUI()
    //{
    //    float fps = 1f / Time.unscaledDeltaTime;
    //    GUI.Label(new Rect(10, 10, 100, 20), ((int)fps) + " FPS");
    //}
}

public enum SlotState
{
    Use, Reload, None, MultiShot
}
