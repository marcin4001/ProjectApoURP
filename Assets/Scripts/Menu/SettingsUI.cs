using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

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
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Button applyButton;
    [SerializeField] private int currentResolutionIndex;
    [SerializeField] private bool fullscreen = true;
    [SerializeField] private bool active = false;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        closeButton.onClick.AddListener(Close);
        leftArrowResolution.onClick.AddListener(OnClickLeftRes);
        rightArrowResolution.onClick.AddListener(OnClickRightRes);
        leftArrowFullscreen.onClick.AddListener(ChangeFullscreen);
        rightArrowFullscreen.onClick.AddListener(ChangeFullscreen);
        applyButton.onClick.AddListener(Apply);
        ValidResolution();
        backgroud.SetActive(false);
        panel.SetActive(false);
    }

    public void Show()
    {
        fullscreen = Screen.fullScreen;
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
        musicSlider.value = GameParam.instance.mainMusicVolume;
        sfxSlider.value = GameParam.instance.sfxVolume;
        
        backgroud.SetActive(true);
        panel.SetActive(true);
        active = true;
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

    public void Apply()
    {
        Vector2Int currentResolution = GameParam.instance.resolutions[currentResolutionIndex];
        Screen.SetResolution(currentResolution.x, currentResolution.y, fullscreen);
        GameParam.instance.currentResolution = currentResolutionIndex;
        GameParam.instance.mainMusicVolume = musicSlider.value;
        GameParam.instance.sfxVolume = sfxSlider.value;
        MusicManager.instance.SetVolumeMainMusic(musicSlider.value);
        MusicManager.instance.SetVolumeSFX(sfxSlider.value);

        PlayerPrefs.SetFloat("mainMusicVolume", GameParam.instance.mainMusicVolume);
        PlayerPrefs.SetFloat("sfxVolume", GameParam.instance.sfxVolume);
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
}
