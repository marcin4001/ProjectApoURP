using UnityEngine;

public class Latrine : MonoBehaviour, IUsableObj
{
    [SerializeField] private bool isOpen = false;
    [SerializeField] private Transform nearPoint;
    [SerializeField] private AudioClip openClip;
    [SerializeField] private AudioClip closeClip;
    private Animator animator;
    private AudioSource source;

    private void Start()
    {
        animator = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
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
        if(source != null )
        {
            if(isOpen)
            {
                source.clip = openClip;
            }
            else
            {
                source.clip= closeClip;
            }
            source.Play();
        }
    }
    
}
