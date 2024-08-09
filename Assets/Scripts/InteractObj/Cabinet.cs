using System.Collections;
using UnityEngine;

public class Cabinet : MonoBehaviour, IUsableObj
{
    [SerializeField] private string isOpenParam = "isOpen";
    [SerializeField] private bool isOpen = false;
    [SerializeField] private Transform nearPoint;
    [SerializeField] private string cabinetName = "cabinet";
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Use()
    {
        StartCoroutine(Open());
    }

    public IEnumerator Open()
    {
        isOpen = true;
        animator.SetBool(isOpenParam, isOpen);
        yield return new WaitForSeconds(0.5f);
        CabinetUI.instance.Show(this);
    }

    public void Close()
    {
        isOpen = false;
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

    public string GetCabinetName()
    {
        return cabinetName;
    }
}
