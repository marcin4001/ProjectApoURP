using System.Collections;
using UnityEngine;

public class SlotMachine : MonoBehaviour, IUsableObj
{
    [SerializeField] private Transform nearPoint;
    [SerializeField] private Animator anim;
    [SerializeField] private string actionParam = "Action";
    [SerializeField] private bool active = false;

    private void Start()
    {
        
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
        if(active)
            return;
        StartCoroutine(DrawSlot());
    }

    private IEnumerator DrawSlot()
    {
        active = true;
        anim.SetTrigger(actionParam);
        yield return new WaitForSeconds(1.5f);
        active = false;
    }
}
