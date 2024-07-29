using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public static HUDController instance;
    [SerializeField] private RectTransform background;
    [SerializeField] private StateButton moveStateButton;
    [SerializeField] private StateButton useStateButton;
    [SerializeField] private StateButton lookStateButton;
    [SerializeField] private Button hideButton;
    [SerializeField] private Button showButton;
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
                moveStateButton.ShoeSelectFrame(); 
                break;
            case PlayerActionState.Use:
                useStateButton.ShoeSelectFrame();
                break;
            case PlayerActionState.Look:
                lookStateButton.ShoeSelectFrame();
                break;
        }
    }

    private void UnselectAllStateButton()
    {
        moveStateButton.HideSelectFrame();
        useStateButton.HideSelectFrame();
        lookStateButton.HideSelectFrame();
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
