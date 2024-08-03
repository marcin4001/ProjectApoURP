using System.Collections;
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
    [SerializeField] private bool clickUp = false;
    [SerializeField] private bool clickDown = false;
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

    public void Show()
    {
        canvas.enabled = true;
        player.SetInMenu(true);
        listItems.normalizedPosition = new Vector2 (0, 1f);
    }

    public void Hide()
    {
        canvas.enabled = false;
        player.SetInMenu(false);
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
}
