using UnityEngine;

public class Trapdoor : MonoBehaviour, IUsableObj
{
    [SerializeField] private Transform nearPoint;
    [SerializeField] private string openParam = "Open";
    [SerializeField] private Animator anim;
    void Start()
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
        anim.SetTrigger(openParam);
    }

}
