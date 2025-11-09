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

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        yesBtn.onClick.AddListener(ClickYes);
        noBtn.onClick.AddListener(ClickNo);
        panel.SetActive(false);
    }

    public void Open(string _message, UnityAction _action)
    {
        panel.SetActive(true);
        textMessage.text = _message;
        action = _action;
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
    }
}
