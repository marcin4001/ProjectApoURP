using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private string separator = "-------------------------------";
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
        cabinet = _cabinet;
        canvas.enabled = true;
        player.SetInMenu(true);
        CreateListCabinet();
        CreateListPlayer();
        if(scrollListItemsCabinet != null)
            scrollListItemsCabinet.ResetPositionList();
        if(scrollListItemsPlayer != null)
            scrollListItemsPlayer.ResetPositionList();
        consoleText.text = string.Empty;
        active = true;
        cabinetNameText.text = cabinet.GetCabinetName();
        CameraMovement.instance.SetBlock(true);
    }

    public void CreateListCabinet()
    {
        foreach(Transform slot in contentCabinet)
        {
            Destroy(slot.gameObject);
        }
        List<SlotItem> listItem = cabinet.GetItems();
        foreach(SlotItem item in listItem)
        {
            SlotItemUI slot = Instantiate(slotPrefab, contentCabinet).GetComponent<SlotItemUI>();
            slot.SetSlot(item);
            slot.SetTypeSlot(SlotUIType.cabinet);
        }
    }

    public void CreateListPlayer()
    {
        foreach (Transform slot in contentPlayer)
        {
            Destroy(slot.gameObject);
        }
        List<SlotItem> listItem = Inventory.instance.GetItems();
        foreach (SlotItem item in listItem)
        {
            SlotItemUI slot = Instantiate(slotPrefab, contentPlayer).GetComponent<SlotItemUI>();
            slot.SetSlot(item);
            slot.SetTypeSlot(SlotUIType.cabinet);
            slot.SetPlayerItem(true);
        }
    }

    public void ReCreateList()
    {
        StartCoroutine(CreateListAfterTime());
    }

    private IEnumerator CreateListAfterTime()
    {
        yield return new WaitForEndOfFrame();
        CreateListCabinet();
        CreateListPlayer();
    }

    public void Hide()
    {
        canvas.enabled = false;
        player.SetInMenu(false);
        active = false;
        cabinet.Close();
        cabinet = null;
        CameraMovement.instance.SetBlock(false);
    }

    public Canvas GetCanvas()
    {
        return canvas;
    }

    public Cabinet GetCabinet()
    {
        return cabinet;
    }

    public void ShowDescription(SlotItem slot)
    {
        Item item = slot.GetItem();
        consoleText.text = $"{item.nameItem}\n{separator}\n{item.description}";
        if (slot.GetAmount() > 1)
            consoleText.text += $"\nAmount: {slot.GetAmount()}";
    }
}
