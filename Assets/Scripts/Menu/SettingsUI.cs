using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Rendering;

public class SettingsUI : MonoBehaviour
{
    public static SettingsUI instance;
    [SerializeField] private GameObject backgroud;
    [SerializeField] private GameObject panel;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button leftArrowResolution;
    [SerializeField] private Button rightArrowResolution;
    [SerializeField] private TextMeshProUGUI resolutionText;
    [SerializeField] private Button leftArrowFullscreen;
    [SerializeField] private Button rightArrowFullscreen;
    [SerializeField] private TextMeshProUGUI fullscreenText;
    [SerializeField] private Button leftArrowVsync;
    [SerializeField] private Button rightArrowVsync;
    [SerializeField] private TextMeshProUGUI vSyncText;
    [SerializeField] private Button leftArrowRetro;
    [SerializeField] private Button rightArrowRetro;
    [SerializeField] private TextMeshProUGUI RetroText;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Button applyButton;
    [SerializeField] private int currentResolutionIndex;
    [SerializeField] private bool fullscreen = true;
    [SerializeField] private bool vSync = true;
    [SerializeField] private bool retro = true;
    [SerializeField] private bool active = false;
    [SerializeField] private TextMeshProUGUI resolutionTitle;
    [SerializeField] private TextMeshProUGUI fullscreenTitle;
    [SerializeField] private TextMeshProUGUI musicVolTitle;
    [SerializeField] private TextMeshProUGUI sfxVolTitle;
    [SerializeField] private TextMeshProUGUI applyTitle;
    private Volume volume;
    private LocalizationCollector loc;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        loc = new LocalizationCollector();
        closeButton.onClick.AddListener(Close);
        leftArrowResolution.onClick.AddListener(OnClickLeftRes);
        rightArrowResolution.onClick.AddListener(OnClickRightRes);
        leftArrowFullscreen.onClick.AddListener(ChangeFullscreen);
        rightArrowFullscreen.onClick.AddListener(ChangeFullscreen);
        leftArrowVsync.onClick.AddListener(ChangeVSync);
        rightArrowVsync.onClick.AddListener(ChangeVSync);
        leftArrowRetro.onClick.AddListener(ChangeRetro);
        rightArrowRetro.onClick.AddListener(ChangeRetro);
        applyButton.onClick.AddListener(Apply);
        ValidResolution();
        backgroud.SetActive(false);
        panel.SetActive(false);
        retro = GameParam.instance.retro;
        SetRetroFX();
    }

    public void Show()
    {
        fullscreen = Screen.fullScreen;
        vSync = QualitySettings.vSyncCount > 0;
        Vector2Int currentResolution = GameParam.instance.resolutions[currentResolutionIndex];
        SetTextResolution(currentResolution);
        if (fullscreen)
        {
            fullscreenText.text = "ON";
        }
        else
        {
            fullscreenText.text = "OFF";
        }
        if(vSync)
        {
            vSyncText.text = "ON";
        }
        else
        {
            vSyncText.text = "OFF";
        }
        musicSlider.value = GameParam.instance.mainMusicVolume;
        sfxSlider.value = GameParam.instance.sfxVolume;
        retro = GameParam.instance.retro;
        if (retro)
        {
            RetroText.text = "ON";
        }
        else
        {
            RetroText.text = "OFF";
        }
        backgroud.SetActive(true);
        panel.SetActive(true);
        active = true;
        SetLocText();
    }

    public void Close()
    {
        backgroud.SetActive(false);
        panel.SetActive(false);
        active = false;
    }

    public void SetTextResolution(Vector2Int res)
    {
        resolutionText.text = $"{res.x}x{res.y}";
    }

    public void OnClickRightRes()
    {
        currentResolutionIndex++;
        if(currentResolutionIndex >= GameParam.instance.resolutions.Length)
            currentResolutionIndex = 0;
        Vector2Int currentResolution = GameParam.instance.resolutions[currentResolutionIndex];
        SetTextResolution(currentResolution);
    }

    public void OnClickLeftRes()
    {
        currentResolutionIndex--;
        if (currentResolutionIndex < 0)
            currentResolutionIndex = GameParam.instance.resolutions.Length - 1;
        Vector2Int currentResolution = GameParam.instance.resolutions[currentResolutionIndex];
        SetTextResolution(currentResolution);
    }

    public void ChangeFullscreen()
    {
        fullscreen = !fullscreen;
        if(fullscreen)
        {
            fullscreenText.text = "ON";
        }
        else
        {
            fullscreenText.text = "OFF";
        }
    }

    public void ChangeVSync()
    {
        vSync = !vSync;
        if (vSync)
        {
            vSyncText.text = "ON";
        }
        else
        {
            vSyncText.text = "OFF";
        }
    }

    public void ChangeRetro()
    {
        retro = !retro;
        if(retro)
        {
            RetroText.text = "ON";
        }
        else
        {
            RetroText.text = "OFF";
        }
    }

    public void Apply()
    {
        Vector2Int currentResolution = GameParam.instance.resolutions[currentResolutionIndex];
        Screen.SetResolution(currentResolution.x, currentResolution.y, fullscreen);
        GameParam.instance.currentResolution = currentResolutionIndex;
        GameParam.instance.mainMusicVolume = musicSlider.value;
        GameParam.instance.sfxVolume = sfxSlider.value;
        MusicManager.instance.SetVolumeMainMusic(musicSlider.value);
        MusicManager.instance.SetVolumeSFX(sfxSlider.value);
        if(vSync)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }

        PlayerPrefs.SetFloat("mainMusicVolume", GameParam.instance.mainMusicVolume);
        PlayerPrefs.SetFloat("sfxVolume", GameParam.instance.sfxVolume);
        if(retro)
        {
            PlayerPrefs.SetInt("retro", 1);
        }
        else
        {
            PlayerPrefs.SetInt("retro", 0);
        }
        GameParam.instance.retro = retro;
        SetRetroFX();
    }

    public void ValidResolution()
    {
        Vector2Int vectorRes = new Vector2Int(Screen.width, Screen.height);
        int foundIndex = System.Array.IndexOf(GameParam.instance.resolutions, vectorRes);

        if(foundIndex != -1)
        {
            currentResolutionIndex = foundIndex;
            GameParam.instance.currentResolution = foundIndex;
            return;
        }

        Vector2Int[] candidates = GameParam.instance.resolutions
            .Where(r => r.x <= vectorRes.x &&  r.y <= vectorRes.y)
            .OrderByDescending(r => r.x * r.y)
            .ToArray();

        if(candidates.Length > 0)
        {
            Vector2Int newRes = candidates[0];
            currentResolutionIndex = System.Array.IndexOf(GameParam.instance.resolutions, newRes);
            GameParam.instance.currentResolution = currentResolutionIndex;
            Screen.SetResolution(newRes.x, newRes.y, fullscreen);
        }
    }

    public bool GetActive()
    {
        return active;
    }

    public void SetLocText()
    {
        string resT = "Resolution";
        string fullscreenT = "Fullscreen";
        string musicT = "MUSIC VOLUME";
        string sfxT = "SFX VOLUME";
        string apply = "APPLY";
        resolutionTitle.text = loc.LoadTranslate(resT);
        fullscreenTitle.text = loc.LoadTranslate(fullscreenT);
        musicVolTitle.text = loc.LoadTranslate(musicT);
        sfxVolTitle.text = loc.LoadTranslate(sfxT);
        applyTitle.text = loc.LoadTranslate(apply);
    }

    private void SetRetroFX()
    {
        volume = FindFirstObjectByType<Volume>();
        if (volume != null)
        {
            volume.profile.TryGet(out PSXPixelizationVolume psx);
            psx.enabled.value = retro;
            if(CameraMovement.instance != null)
                CameraMovement.instance.SetAntialiasing(!retro);
        }
    }
}
