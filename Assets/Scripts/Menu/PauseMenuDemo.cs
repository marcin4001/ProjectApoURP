using UnityEngine;
using UnityEngine.UI;

public class PauseMenuDemo : MonoBehaviour
{
    public static PauseMenuDemo instance;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private bool active = false;
    [SerializeField] private string messageQuit;
    private Canvas canvas;
    private PlayerController player;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
        canvas = GetComponent<Canvas>();
        canvas.enabled = false;
        resumeButton.onClick.AddListener(Hide);
        quitButton.onClick.AddListener(OnClickQuit);
        settingsButton.onClick.AddListener(OnClickSettings);
    }

    public bool GetActive()
    {
        return active;
    }

    public void Show()
    {
        canvas.enabled = true;
        Time.timeScale = 0;
        HUDController.instance.SetActiveCanvas(false);
        player.SetInMenu(true);
        active = true;
    }

    public void Hide()
    {
        canvas.enabled = false;
        Time.timeScale = 1;
        HUDController.instance.SetActiveCanvas(true);
        player.SetInMenu(false);
        active = false;
    }

    private void OnClickSettings()
    {
        SettingsUI.instance.Show();
    }

    private void OnClickQuit()
    {
        MessagePanel.instance.Open(messageQuit, Quit);
    }

    private void Quit()
    {
        Debug.Log("Exit");
        Application.Quit();
    }
}
