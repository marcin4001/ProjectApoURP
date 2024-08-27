using System.Collections;
using UnityEditorInternal.VersionControl;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollListController : MonoBehaviour
{
    [SerializeField] private EventTrigger upEventTrigger;
    [SerializeField] private EventTrigger downEventTrigger;
    [SerializeField] private bool clickUp = false;
    [SerializeField] private bool clickDown = false;
    private ScrollRect listItems;
    private Coroutine currentCoroutine;
    private float scrollSpeed = 700f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        listItems = GetComponent<ScrollRect>();
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
        downEventTrigger.triggers.Add(entryStopScroll);
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
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);
    }

    public void ResetPositionList()
    {
        listItems.normalizedPosition = new Vector2(0, 1f);
    }

    private IEnumerator Scroll()
    {
        float speedFactor = 1f / listItems.content.sizeDelta.y;
        while (clickDown)
        {
            Vector2 pos = listItems.normalizedPosition;
            pos.y -= scrollSpeed * Time.deltaTime / listItems.content.sizeDelta.y;
            listItems.normalizedPosition = pos;
            yield return new WaitForEndOfFrame();
        }

        while (clickUp)
        {
            Vector2 pos = listItems.normalizedPosition;
            pos.y += scrollSpeed * Time.deltaTime / listItems.content.sizeDelta.y;
            listItems.normalizedPosition = pos;
            yield return new WaitForEndOfFrame();
        }
    }
}
