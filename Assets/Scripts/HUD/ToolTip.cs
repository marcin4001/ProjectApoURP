using System.Collections;
using TMPro;
using UnityEngine;

public class ToolTip : MonoBehaviour
{
    public static ToolTip instance;
    [SerializeField] private RectTransform background;
    [SerializeField] private RectTransform textTransform;
    [SerializeField] private TextMeshProUGUI text;
    private Coroutine coroutine;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        instance = this;
        background.gameObject.SetActive(false);
    }

    public void SetText(string tooltipText)
    {
        text.text = $" {tooltipText}";
        background.gameObject.SetActive(true);
        StartCoroutine(Resize());
        if(coroutine != null)
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(Hide());
    }

    private IEnumerator Resize()
    {
        yield return new WaitForEndOfFrame();
        Vector2 newSize = textTransform.sizeDelta;
        newSize.x += 10f;
        background.sizeDelta = newSize;
    }

    private IEnumerator Hide()
    {
        yield return new WaitForSeconds(2);
        background.gameObject.SetActive(false);
    }

    public void ExitHide()
    {
        background.gameObject.SetActive(false);
        if (coroutine != null)
            StopCoroutine(coroutine);
        coroutine = null;
    }
}
