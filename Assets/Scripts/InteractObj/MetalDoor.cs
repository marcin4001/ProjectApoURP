using System.Collections;
using UnityEngine;

public class MetalDoor : MonoBehaviour, IUsableObj
{
    [SerializeField] private Transform nearPoint;
    [SerializeField] private Animator animatorDoor;
    [SerializeField] private string openParam = "Open";
    [SerializeField] private string openStartParam = "OpenStart";
    [SerializeField] private string doorID;
    [SerializeField] private AudioClip openClip;
    private AudioSource source;
    private PlayerController playerController;
    private bool isLock = true;

    private void Start()
    {
        playerController = FindFirstObjectByType<PlayerController>();
        source = GetComponent<AudioSource>();
        if (GameParam.instance.DoorOnList(doorID))
        {
            animatorDoor.SetTrigger(openStartParam);
            Destroy(this);
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
        if(isLock)
        {
            HUDController.instance.AddConsolelog("The door is locked because");
            HUDController.instance.AddConsolelog("there’s no power.");
            return;
        }
        if(animatorDoor != null)
            StartCoroutine(OpenDoor());
    }

    public void OpenDoorTerminal()
    {
        isLock = false;
        if (animatorDoor != null)
            StartCoroutine(OpenDoor());
    }

    private IEnumerator OpenDoor()
    {
        if(source != null)
        {
            source.clip = openClip;
            source.Play();
        }
        playerController.SetBlock(true);
        animatorDoor.SetTrigger(openParam);
        yield return new WaitForSeconds(1);
        playerController.SetBlock(false);
        GameParam.instance.AddOpenDoor(doorID);
        yield return new WaitForEndOfFrame();
        Destroy(this);
    }

    public void Unlock()
    {
        isLock = false;
    }
}
