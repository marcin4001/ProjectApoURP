using System.Collections;
using TMPro;
using UnityEngine;

public class BackgroundNPC : MonoBehaviour, IUsableObj
{
    [SerializeField] private TextMeshPro dialogueText;
    [SerializeField] private Transform nearPoint;
    [SerializeField] private float timeToHide = 3f;
    [SerializeField] private string[] texts;
    private int nextIndex = 0;
    private Coroutine coroutine;
    void Start()
    {
        dialogueText.gameObject.SetActive(false);
    }
    public bool CanUse()
    {
        return true;
    }

    public GameObject GetMainGameObject()
    {
        return gameObject;
    }

    public Vector3 GetNearPoint()
    {
        return nearPoint.position;
    }

    public void Use()
    {
        if (texts.Length > 0)
        {
            if (coroutine != null)
                StopCoroutine(coroutine);
            dialogueText.gameObject.SetActive(true);
            dialogueText.text = texts[nextIndex];
            nextIndex++;
            if(nextIndex == texts.Length)
                nextIndex = 0;
            coroutine = StartCoroutine(HideText());
        }
    }

    private IEnumerator HideText()
    {
        yield return new WaitForSeconds(timeToHide);
        dialogueText.gameObject.SetActive(false);
    }

}
