using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CabinetUI : MonoBehaviour
{
    public static CabinetUI instance;
    [SerializeField] private Button closeButton;
    [SerializeField] private TextMeshProUGUI consoleText;
    [SerializeField] private TextMeshProUGUI cabinetNameText;
    [SerializeField] private Transform contentCabinet;
    [SerializeField] private Transform contentPlayer;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private bool active = false;
    [SerializeField] private ScrollListController scrollListItemsCabinet;
    [SerializeField] private ScrollListController scrollListItemsPlayer;
    private Canvas canvas;
    private PlayerController player;
    private Cabinet cabinet;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        canvas = GetComponent<Canvas>();
        player = FindFirstObjectByType<PlayerController>();
        canvas.enabled = false;
        closeButton.onClick.AddListener(Hide);
    }

    public bool GetActive()
    {
        return active;
    }

    public void Show(Cabinet _cabinet)
    {
        canvas.enabled = true;
        player.SetInMenu(true);
        if(scrollListItemsCabinet != null)
            scrollListItemsCabinet.ResetPositionList();
        if(scrollListItemsPlayer != null)
            scrollListItemsPlayer.ResetPositionList();
        consoleText.text = string.Empty;
        active = true;
        cabinet = _cabinet;
        cabinetNameText.text = cabinet.GetCabinetName();
    }

    public void Hide()
    {
        canvas.enabled = false;
        player.SetInMenu(false);
        active = false;
        cabinet.Close();
        cabinet = null;
    }
}
