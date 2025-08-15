using UnityEngine;
using UnityEngine.InputSystem;

public class BaseDoorOutside : MonoBehaviour, IUsableObj
{
    [SerializeField] private Transform nearpoint;
    [SerializeField] private GameObject mainObj;
    [SerializeField] private string openParam = "Open";
    [SerializeField] private int keyID;
    private bool isLock = true;
    private Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public bool CanUse()
    {
        return true;
    }

    public GameObject GetMainGameObject()
    {
        return mainObj;
    }

    public Vector3 GetNearPoint()
    {
        return nearpoint.position;
    }

    public void Use()
    {
        Debug.Log("isLock = " + isLock);
        if (isLock)
            return;
        anim.SetTrigger(openParam);

    }

    public bool CheckKey(int _keyID)
    {
        return keyID == _keyID;
    }

    public void Unlock()
    {
        isLock = false;
    }

    public bool IsLocked()
    {
        return isLock;
    }
}
