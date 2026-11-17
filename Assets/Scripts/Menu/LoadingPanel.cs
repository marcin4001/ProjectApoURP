using TMPro;
using UnityEngine;

public class LoadingPanel : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI loadingText;
    private LocalizationCollector loc;
    void Start()
    {
        loc = new LocalizationCollector();
        if(loadingText != null)
        {
            string loadingT = "Loading...";
            loadingText.text = loc.LoadTranslate(loadingT);
        }
        panel.SetActive(false);
    }

    public void Show()
    {
        panel.SetActive(true);
    }
}
