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
    [SerializeField] private int indexTheme = 0;
    private MainInputSystem inputActions;

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
        MusicManager.instance.SetMaxVolume(GameParam.instance.maxVolumeTheme);
        MusicManager.instance.SetTheme(indexTheme);
        playButton.onClick.AddListener(OnClickPlay);
        settingsButton.onClick.AddListener(OnClickSettings);
        quitButton.onClick.AddListener(OnClickQuit);
        GameParam.instance.mainMusicVolume = PlayerPrefs.GetFloat("mainMusicVolume", 1f);
        GameParam.instance.sfxVolume = PlayerPrefs.GetFloat("sfxVolume", 1f);
    }

    private void OnClickPlay()
    {
        GameParam.instance.SetStartParam();
        if(Inventory.instance != null)
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
        Debug.Log("Exit");
        Application.Quit();
    }

    public void OnEscClick(InputAction.CallbackContext ctx)
    {
        if (SettingsUI.instance.GetActive())
        {
            SettingsUI.instance.Close();
            return;
        }
    }
}
