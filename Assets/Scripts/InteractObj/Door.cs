using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.AI;

public class Door : MonoBehaviour, IUsableObj
{
    [SerializeField] private HouseRoof roof;
    [SerializeField] private Transform root;
    [SerializeField] private Transform slot1;
    [SerializeField] private Transform slot2;
    [SerializeField] private float maxDistance = 1.0f;
    [SerializeField] private float closeAngle = 0f;
    [SerializeField] private float openAngle = -90f;
    [SerializeField] private bool isOpen = false;
    private PlayerController player;
    private NavMeshObstacle obstacle;
    private Coroutine currentCoroutine;
    private float alpha = 0f;

    void Start()
    {
        player = FindAnyObjectByType<PlayerController>();
        obstacle = GetComponent<NavMeshObstacle>();
    }

    public void Use()
    {
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);
        isOpen = !isOpen;
        if (isOpen)
            currentCoroutine = StartCoroutine(OpenTask());
        else
            currentCoroutine = StartCoroutine(CloseTask());
        if(roof != null)
        {
            if(isOpen)
                roof.Hide();
            else
                roof.Show();
        }
    }

    public Vector3 GetRootPosition()
    {
        return root.position;
    }

    private IEnumerator OpenTask()
    {
        obstacle.enabled = false;
        while(alpha <= 1f)
        {
            alpha += Time.deltaTime;
            Vector3 doorRotation = transform.localEulerAngles;
            doorRotation.y = Mathf.Lerp(closeAngle, openAngle, alpha);
            transform.localEulerAngles = doorRotation;
            yield return new WaitForEndOfFrame();
        }
        obstacle.enabled = true;
    }
    private IEnumerator CloseTask()
    {
        obstacle.enabled = false;
        while(alpha >= 0f)
        {
            alpha -= Time.deltaTime;
            Vector3 doorRotation = transform.localEulerAngles;
            doorRotation.y = Mathf.Lerp(closeAngle, openAngle, alpha);
            transform.localEulerAngles = doorRotation;
            yield return new WaitForEndOfFrame();
        }
        obstacle.enabled = true;
    }

    public Vector3 GetNearPoint()
    {
        float distanceToSlot1 = Vector3.Distance(player.transform.position, slot1.transform.position);
        float distanceToSlot2 = Vector3.Distance(player.transform.position, slot2.transform.position);

        if (distanceToSlot1 < distanceToSlot2)
            return slot1.position;
        else
            return slot2.position;
    }

    public GameObject GetMainGameObject()
    {
        return gameObject;
    }

    public bool IsOpen()
    {
        return isOpen;
    }
}
