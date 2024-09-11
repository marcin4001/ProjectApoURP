using UnityEngine;
using UnityEngine.EventSystems;

public class ToolTipObj : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private string tooltipText;
    public void OnPointerEnter(PointerEventData eventData)
    {
        ToolTip.instance.SetText(tooltipText);
    }
}
