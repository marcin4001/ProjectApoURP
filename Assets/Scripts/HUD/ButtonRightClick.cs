using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ButtonRightClick : MonoBehaviour, IPointerClickHandler
{
    public UnityEvent OnClickRight;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            OnClickRight.Invoke();
        }
    }

}
