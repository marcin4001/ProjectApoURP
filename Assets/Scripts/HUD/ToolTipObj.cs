using UnityEngine;
using UnityEngine.EventSystems;

public class ToolTipObj : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string tooltipText;

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
        ToolTip.instance.SetText(tooltipText);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ToolTip.instance.ExitHide();
    }
}
