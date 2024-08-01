using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private Image slotItemImage;
    [SerializeField] private List<Item> slots = new List<Item>();
    [SerializeField] private int currentSlotIndex = 0;
    private PlayerController player;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
        showButton.onClick.AddListener(Show);
        hideButton.onClick.AddListener(Hide);
        slotButton.onClick.AddListener(OnClickSlot);
        leftButton.onClick.AddListener(OnClickLeftButton);
        rightButton.onClick.AddListener(OnClickRigtButton);
        slotChangeStateButton.OnClickRight.AddListener(ChangeStateSlot);
        consoleText.text = string.Empty;
        slotStateText.text = slotState.ToString();
        Show();
        SetItemSlot();
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
        switch(slotState)
        {
            case SlotState.Use:
                player.StartUsingItem();
                break;
            case SlotState.Reload:
                player.ReloadGun();
                break;
        }
    }

    public void ChangeStateSlot()
    {
        Item item = GetCurrentItem();
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
        if(currentSlotIndex == slots.Count)
            currentSlotIndex = 0;
        SetItemSlot();
    }
    
    public void OnClickLeftButton()
    {
        currentSlotIndex--;
        if(currentSlotIndex < 0)
            currentSlotIndex = slots.Count - 1;
        SetItemSlot();
    }

    public Item GetCurrentItem()
    {
        return slots[currentSlotIndex];
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

    public void SetItemSlot()
    {
        Item item = slots[currentSlotIndex];
        if(item == null)
        {
            slotState = SlotState.None;
            slotItemImage.enabled = false;
            player.ShowWeapon(null);
        }
        else
        {
            slotState = SlotState.Use;
            slotItemImage.enabled = true;
            slotItemImage.overrideSprite = item.uiSprite;
            player.ShowWeapon(item);
        }
        slotStateText.text = slotState.ToString();
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
}

public enum SlotState
{
    Use, Reload, None
}
