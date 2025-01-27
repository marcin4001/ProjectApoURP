using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TradeUI : MonoBehaviour
{
    public static TradeUI instance;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button tradeButton;
    [SerializeField] private TextMeshProUGUI consoleText;
    [SerializeField] private TextMeshProUGUI npcNameText;
    [SerializeField] private TextMeshProUGUI moneyPlayerText;
    [SerializeField] private TextMeshProUGUI moneyNPCText;
    [SerializeField] private Transform contentNPC;
    [SerializeField] private Transform contentPlayer;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private string separator = "-------------------------------";
    [SerializeField] private bool active = false;
    [SerializeField] private ScrollListController scrollListItemsNPC;
    [SerializeField] private ScrollListController scrollListItemsPlayer;
    [SerializeField] private SlotItem moneyPlayer;
    [SerializeField] private SlotItem moneyNPC;
    private Canvas canvas;
    private PlayerController player;
    private DialogueNPC nPC;
    private int moneyID = 202;

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
        tradeButton.onClick.AddListener(Show);
    }

    public bool GetActive()
    {
        return active;
    }

    public void Show()
    {
        if(nPC == null)
            return;
        if(!nPC.IsSeller())
        {
            nPC.ShowNoSellReply();
            return;
        }
        canvas.enabled = true;
        CreateListNPC();
        CreateListPlayer();
        if (scrollListItemsNPC != null)
            scrollListItemsNPC.ResetPositionList();
        if (scrollListItemsPlayer != null)
            scrollListItemsPlayer.ResetPositionList();
        consoleText.text = string.Empty;
        active = true;
        npcNameText.text = nPC.GetNPCName();
    }

    public void CreateListNPC()
    {
        moneyNPCText.text = "$0";
        foreach (Transform slot in contentNPC)
        {
            Destroy(slot.gameObject);
        }
        List<SlotItem> listItem = nPC.GetItems();
        foreach (SlotItem item in listItem)
        {
            if(item.GetItem().id == moneyID)
            {
                moneyNPC = item;
                moneyNPCText.text = $"${moneyNPC.GetAmount()}";
                continue;
            }
            SlotItemUI slot = Instantiate(slotPrefab, contentNPC).GetComponent<SlotItemUI>();
            slot.SetSlot(item);
            slot.SetTypeSlot(SlotUIType.trade);
        }
    }

    public void CreateListPlayer()
    {
        moneyPlayerText.text = "$0";
        foreach (Transform slot in contentPlayer)
        {
            Destroy(slot.gameObject);
        }
        List<SlotItem> listItem = Inventory.instance.GetItems();
        foreach (SlotItem item in listItem)
        {
            if (item.GetItem().id == moneyID)
            {
                moneyPlayer = item;
                moneyPlayerText.text = $"${moneyPlayer.GetAmount()}";
                continue;
            }
            SlotItemUI slot = Instantiate(slotPrefab, contentPlayer).GetComponent<SlotItemUI>();
            slot.SetSlot(item);
            slot.SetTypeSlot(SlotUIType.trade);
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
        CreateListNPC();
        CreateListPlayer();
    }

    public void Hide()
    {
        canvas.enabled = false;
        active = false;
    }

    public Canvas GetCanvas()
    {
        return canvas;
    }

    public DialogueNPC GetNPC()
    {
        return nPC;
    }

    public void SetNPC(DialogueNPC npc)
    {
        nPC = npc;
    }

    public void ShowDescription(SlotItem slot)
    {
        Item item = slot.GetItem();
        consoleText.text = $"{item.nameItem}\n{separator}\n{item.description}";
        if (slot.GetAmount() > 1)
            consoleText.text += $"\nAmount: {slot.GetAmount()}";
    }
}
