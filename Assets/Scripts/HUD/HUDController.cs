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
    [Header("Hide/Show HUD Buttons")]
    [SerializeField] private Button hideButton;
    [SerializeField] private Button showButton;
    [Header("Console")]
    [SerializeField] private TextMeshProUGUI consoleText;
    [SerializeField] private List<string> consoleLogs = new List<string>();
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
        consoleText.text = string.Empty;
        Show();
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
