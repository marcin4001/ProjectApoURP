using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI instance;
    [SerializeField] private Button exitButton;
    [SerializeField] private GameObject optionButtonPrefab;
    [SerializeField] private TextMeshProUGUI replyText;
    [SerializeField] private Transform parentOptions;
    [SerializeField] private bool active;
    private Canvas canvas;
    private PlayerController player;

    private void Awake()
    {
        instance = this;
        canvas = GetComponent<Canvas>();
        player = FindFirstObjectByType<PlayerController>();
        canvas.enabled = false;
    }

    void Start()
    {
        exitButton.onClick.AddListener(Hide);
    }

    public void Show()
    {
        canvas.enabled = true;
        active = true;
        player.SetInMenu(true);
    }

    public void Hide()
    {
        canvas.enabled = false;
        active = false;
        player.SetInMenu(false);
    }

    public void SetReply(string reply)
    {
        replyText.text = reply;
    }

    public void CreateListOptions(List<DialogueOption> options)
    {
        foreach(Transform option in parentOptions)
        {
            Destroy(option.gameObject);
        }
        foreach(DialogueOption option in options)
        {
            DialogueOptionButton optionButton =  Instantiate(optionButtonPrefab, parentOptions).GetComponent<DialogueOptionButton>();
            optionButton.Init(option);
        }
    }

    public bool GetActive()
    {
        return active;
    }

}
