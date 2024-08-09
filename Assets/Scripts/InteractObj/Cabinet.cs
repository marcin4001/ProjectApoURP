using UnityEngine;

public class Cabinet : MonoBehaviour, IUsableObj
{
    [SerializeField] private string isOpenParam = "isOpen";
    [SerializeField] private bool isOpen = false;
    [SerializeField] private Transform nearPoint;
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Use()
    {
        isOpen = !isOpen;
        animator.SetBool(isOpenParam, isOpen);
    }

    public Vector3 GetNearPoint()
    {
        if(nearPoint != null)
            return nearPoint.position;
        return transform.position;
    }

    public GameObject GetMainGameObject()
    {
        return gameObject;
    }
}
