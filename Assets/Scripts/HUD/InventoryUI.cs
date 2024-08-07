using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI instance;
    [SerializeField] private EventTrigger upEventTrigger;
    [SerializeField] private EventTrigger downEventTrigger;
    [SerializeField] private Button closeButton;
    [SerializeField] private ScrollRect listItems;
    [SerializeField] private TextMeshProUGUI consoleText;
    [SerializeField] private Transform content;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private SlotDropUI[] slotsDrop;
    [SerializeField] private bool clickUp = false;
    [SerializeField] private bool clickDown = false;
    [SerializeField] private bool active = false;
    private Canvas canvas;
    private PlayerController player;
    private Coroutine currentCoroutine;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        canvas = GetComponent<Canvas>();
        player = FindFirstObjectByType<PlayerController>();
        closeButton.onClick.AddListener(Hide);

        EventTrigger.Entry entryUp = new EventTrigger.Entry();
        entryUp.eventID = EventTriggerType.PointerDown;
        entryUp.callback.AddListener(data => OnClickUp());
        upEventTrigger.triggers.Add(entryUp);

        EventTrigger.Entry entryDown = new EventTrigger.Entry();
        entryDown.eventID = EventTriggerType.PointerDown;
        entryDown.callback.AddListener(data => OnClickDown());
        downEventTrigger.triggers.Add(entryDown);

        EventTrigger.Entry entryStopScroll = new EventTrigger.Entry();
        entryStopScroll.eventID = EventTriggerType.PointerUp;
        entryStopScroll.callback.AddListener(data => StopScroll());
        upEventTrigger.triggers.Add(entryStopScroll);
        downEventTrigger.triggers.Add (entryStopScroll);
        canvas.enabled = false;
    }

    public bool GetActive()
    {
        return active;
    }

    public void Show()
    {
        canvas.enabled = true;
        player.SetInMenu(true);
        CreateListItem();
        UpdateAmountTextSlotsDrop();
        listItems.normalizedPosition = new Vector2 (0, 1f);
        consoleText.text = string.Empty;
        active = true;
    }

    public void Hide()
    {
        canvas.enabled = false;
        player.SetInMenu(false);
        active = false;
    }

    public Canvas GetCanvas()
    {
        return canvas;
    }

    public void CreateListItem()
    {
        foreach(Transform slot in content.transform)
        {
            Destroy(slot.gameObject);
        }
        List<SlotItem> slotItems = Inventory.instance.GetItems();
        foreach(SlotItem item in slotItems)
        {
            SlotItemUI slot = Instantiate(slotPrefab, content).GetComponent<SlotItemUI>();
            slot.SetSlot(item);
        }
    }


    public void UpdateAmountTextSlotsDrop()
    {
        for(int i = 0; i < slotsDrop.Length; i++)
            slotsDrop[i].UpdateAmountText();
    }

    public void OnClickDown()
    {
        clickDown = true;
        currentCoroutine = StartCoroutine(Scroll());
    }

    public void OnClickUp()
    {
        clickUp = true;
        currentCoroutine = StartCoroutine(Scroll());
    }

    public void StopScroll()
    {
        clickDown = false;
        clickUp = false;
        if(currentCoroutine != null)
            StopCoroutine(currentCoroutine);
    }
 
    private IEnumerator Scroll()
    {
        while (clickDown)
        {
            Vector2 pos = listItems.normalizedPosition;
            pos.y -= Time.deltaTime;
            listItems.normalizedPosition = pos;
            yield return null;
        }

        while (clickUp)
        {
            Vector2 pos = listItems.normalizedPosition;
            pos.y += Time.deltaTime;
            listItems.normalizedPosition = pos;
            yield return null;
        }
    }

    public void ShowDescription(SlotItem slot)
    {
        Item item = slot.GetItem();
        consoleText.text = $"{item.nameItem}\n--------------------------\n{item.description}";
        if (slot.GetAmount() > 1)
            consoleText.text += $"\nAmount: {slot.GetAmount()}";
    }
}
