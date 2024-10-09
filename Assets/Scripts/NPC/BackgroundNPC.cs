using System.Collections;
using TMPro;
using UnityEngine;

public class BackgroundNPC : MonoBehaviour, IUsableObj
{
    [SerializeField] private TextMeshPro dialogueText;
    [SerializeField] private Transform nearPoint;
    [SerializeField] private float timeToHide = 3f;
    [SerializeField] private string[] texts;
    [SerializeField] private bool haveRifle = false;
    [SerializeField] private string rifleLayer = "Rifle";
    private int nextIndex = 0;
    private Coroutine coroutine;
    private Animator animator;
    void Start()
    {
        dialogueText.gameObject.SetActive(false);
        animator = GetComponentInChildren<Animator>();
        if(haveRifle)
        {
            int rifleIndex = animator.GetLayerIndex(rifleLayer);
            animator.SetLayerWeight(rifleIndex, 1f);
        }
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
            dialogueText.transform.rotation = Quaternion.LookRotation(CameraMovement.instance.GetTransformCamera().forward);
        }
    }

    private IEnumerator HideText()
    {
        yield return new WaitForSeconds(timeToHide);
        dialogueText.gameObject.SetActive(false);
    }

}
