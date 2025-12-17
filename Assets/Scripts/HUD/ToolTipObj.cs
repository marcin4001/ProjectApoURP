using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToolTipObj : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string tooltipText;
    [SerializeField] private bool showAfterTime;
    private Coroutine coroutine;

    public string GetText()
    {
        return tooltipText;
    }

    public void SetText(string text)
    {
        tooltipText = text;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (showAfterTime)
        {
            if(coroutine != null)
                StopCoroutine(coroutine);
            coroutine = StartCoroutine(ShowAfterTime());
            return;
        }
        ToolTip.instance.SetText(tooltipText);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ToolTip.instance.ExitHide();
        if (showAfterTime)
        {
            if (coroutine != null)
                StopCoroutine(coroutine);
        }
    }

    private IEnumerator ShowAfterTime()
    {
        yield return new WaitForSeconds(1f);
        ToolTip.instance.SetText(tooltipText);
    }
}
