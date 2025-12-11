using UnityEngine;

public class Latrine : MonoBehaviour, IUsableObj
{
    [SerializeField] private bool isOpen = false;
    [SerializeField] private Transform nearPoint;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
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
        isOpen = !isOpen;
        animator.SetBool("isOpen", isOpen);
    }
    
}
