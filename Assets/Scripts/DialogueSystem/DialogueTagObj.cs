using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueTagObj : MonoBehaviour
{
    [SerializeField] private float timeToDestroy = 4f;
    [SerializeField] private float counterToDestroy = 0f;
    [SerializeField] private Transform headPos;
    private RectTransform textRect;

    public void Init(Transform _headPos, string text, bool angry = false)
    {
        textRect = GetComponent<RectTransform>();
        headPos = _headPos;
        GetComponent<TextMeshProUGUI>().text = text;
        GetComponent<TextMeshProUGUI>().color = GameParam.instance.normalDialogueTag;
        if(angry)
            GetComponent<TextMeshProUGUI>().color = GameParam.instance.angryDialogueTag;
        StartCoroutine(Showing());
    }

    private IEnumerator Showing()
    {
        Camera cam = DialogueTag.instance.GetCamera();
        RectTransform canvas = DialogueTag.instance.GetCanvasRect();
        while (counterToDestroy < timeToDestroy)
        {
            Vector3 posOnScreen = cam.WorldToScreenPoint(headPos.position);
            Vector2 posText;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, posOnScreen, null, out posText);
            textRect.anchoredPosition = posText;
            yield return new WaitForEndOfFrame();
            counterToDestroy += Time.deltaTime;
        }
        Destroy(gameObject);
    }
}
