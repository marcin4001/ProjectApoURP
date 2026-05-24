using System.IO;
using Steamworks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DemoMenuUI : MonoBehaviour
{
    [SerializeField] private string sceneStartName = "MainScene";
    [SerializeField] private Button playButton;
    [SerializeField] private Button loadButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button guideButton;
    [SerializeField] private Button steamButton;
    [SerializeField] private Button loadOldSave;
    [SerializeField] private int indexTheme = 0;
    [SerializeField] private bool inDemo = false;
    [SerializeField] private string guideURL = "https://drive.google.com/file/d/1cEXWEQmxcKytIYpjPMdWPtfHDkKfMPUp/view?usp=sharing";
    [SerializeField] private string steamURL = "https://store.steampowered.com/app/4546490/Arkansas_2125/";
    [SerializeField] private string messageQuit;
    [SerializeField] public BookProfile book;
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
        loadButton.onClick.AddListener(OnClickLoad);
        creditsButton.onClick.AddListener(OnClickCredits);
        settingsButton.onClick.AddListener(OnClickSettings);
        quitButton.onClick.AddListener(OnClickQuit);
        guideButton.onClick.AddListener(OnClickGuide);
        loadOldSave.onClick.AddListener(OnClickLoadOld);
        GameParam.instance.mainMusicVolume = PlayerPrefs.GetFloat("mainMusicVolume", 1f);
        GameParam.instance.sfxVolume = PlayerPrefs.GetFloat("sfxVolume", 1f);
        steamButton.onClick.AddListener(OnClickSteam);
        if (SteamManager.Initialized)
        {
            string name = SteamFriends.GetPersonaName();
            Debug.Log(name);
            //SteamUserStats.ResetAllStats(true);
            //SteamUserStats.StoreStats();
        }
        HideLoadOldSave();
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

    private void OnClickLoad()
    {
        if(inDemo)
            return;
        LoadPanelUI.instance.Show();
    }

    private void OnClickLoadOld()
    {
        SaveManager.instance.LoadOld();
    }

    private void OnClickCredits()
    {
        BookReader.instance.Show(book);
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

        if (LoadPanelUI.instance.GetActive())
        {
            LoadPanelUI.instance.Close();
            return;
        }
    }

    public void OnClickGuide()
    {
        Application.OpenURL(guideURL);
    }

    public void OnClickSteam()
    {
        Application.OpenURL(steamURL);
    }

    public void HideLoadOldSave()
    {
        string folderPath = Application.persistentDataPath + "/Save";
        if (!Directory.Exists(folderPath))
        {
            Destroy(loadOldSave.gameObject);
        }
    }
}
