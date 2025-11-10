using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DemoMenuUI : MonoBehaviour
{
    [SerializeField] private string sceneStartName = "MainScene";
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button guideButton;
    [SerializeField] private int indexTheme = 0;
    [SerializeField] private bool inDemo = false;
    [SerializeField] private string guideURL = "https://drive.google.com/file/d/1cEXWEQmxcKytIYpjPMdWPtfHDkKfMPUp/view?usp=sharing";
    [SerializeField] private string messageQuit;
    private MainInputSystem inputActions;
    private StatsPanelMenu statsPanelMenu;

    private void Awake()
    {
        inputActions = new MainInputSystem();
        inputActions.Player.Pause.performed += OnEscClick;
        inputActions.Enable();
        
    }

    private void OnEnable()
    {
        inputActions.Player.Pause.performed += OnEscClick;
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Player.Pause.performed -= OnEscClick;
        inputActions.Disable();
    }

    void Start()
    {
        statsPanelMenu = FindFirstObjectByType<StatsPanelMenu>();
        MusicManager.instance.SetMaxVolume(GameParam.instance.maxVolumeTheme);
        MusicManager.instance.SetTheme(indexTheme);
        playButton.onClick.AddListener(OnClickPlay);
        settingsButton.onClick.AddListener(OnClickSettings);
        quitButton.onClick.AddListener(OnClickQuit);
        guideButton.onClick.AddListener(OnClickGuide);
        GameParam.instance.mainMusicVolume = PlayerPrefs.GetFloat("mainMusicVolume", 1f);
        GameParam.instance.sfxVolume = PlayerPrefs.GetFloat("sfxVolume", 1f);
    }

    private void OnClickPlay()
    {
        statsPanelMenu.Open();
    }

    public void LoadNewGame()
    {
        GameParam.instance.SetStartParam();
        GameParam.instance.inDemo = inDemo;
        if (Inventory.instance != null)
            Inventory.instance.Clear();
        //PickUpObjList.instance.CopyList();
        PickUpObjList.instance.Clear();
        KilledEnemiesList.instance.ClearList();
        NPCObjList.instance.ClearList();
        ListCabinet.instance.CopyList();
        ListOffers.instance.CopyList();
        QuestController.instance.ClearList();
        SceneManager.LoadScene(sceneStartName);
    }

    private void OnClickSettings()
    {
        SettingsUI.instance.Show();
    }

    private void OnClickQuit()
    {
        MessagePanel.instance.Open(messageQuit, Quit);
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Exit");
    }

    public void OnEscClick(InputAction.CallbackContext ctx)
    {
        if (SettingsUI.instance.GetActive())
        {
            SettingsUI.instance.Close();
            return;
        }
    }

    public void OnClickGuide()
    {
        Application.OpenURL(guideURL);
    }
}
