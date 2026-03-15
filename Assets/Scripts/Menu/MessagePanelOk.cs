using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MessagePanelOk : MonoBehaviour
{
    public static MessagePanelOk instance;
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI textMessage;
    [SerializeField] private Button okBtn;
    [SerializeField] private bool active = false;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        okBtn.onClick.AddListener(ClickOk);
        panel.SetActive(false);
    }

    public void Open(string _message)
    {
        panel.SetActive(true);
        textMessage.text = _message;
        active = true;
    }

    public void ClickOk()
    {
        panel.SetActive(false);
        active = false;
    }

    public bool GetActive()
    {
        return active;
    }
}
