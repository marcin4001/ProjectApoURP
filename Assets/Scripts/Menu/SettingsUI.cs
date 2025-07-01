using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    public static SettingsUI instance;
    [SerializeField] private GameObject backgroud;
    [SerializeField] private GameObject panel;
    [SerializeField] private Button closeButton;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        closeButton.onClick.AddListener(Close);
        backgroud.SetActive(false);
        panel.SetActive(false);
    }

    public void Show()
    {
        backgroud.SetActive(true);
        panel.SetActive(true);
    }

    public void Close()
    {
        backgroud.SetActive(false);
        panel.SetActive(false);
    }
}
