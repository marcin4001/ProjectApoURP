using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MessagePanel : MonoBehaviour
{
    public static MessagePanel instance;
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI textMessage;
    [SerializeField] private Button noBtn;
    [SerializeField] private Button yesBtn;
    [SerializeField] private UnityAction action;
    [SerializeField] private bool active = false;
    private LocalizationCollector loc;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        yesBtn.onClick.AddListener(ClickYes);
        noBtn.onClick.AddListener(ClickNo);
        panel.SetActive(false);
        loc = new LocalizationCollector();
    }

    public void Open(string _message, UnityAction _action)
    {
        //loc.Collect(_message);
        panel.SetActive(true);
        textMessage.text = loc.LoadTranslate(_message);
        action = _action;
        active = true;
    }

    public void ClickYes()
    {
        action?.Invoke();
        action = null;  
    }

    public void ClickNo()
    {
        action = null;
        panel.SetActive(false);
        active = false;
    }

    public bool GetActive()
    {
        return active;
    }
}
