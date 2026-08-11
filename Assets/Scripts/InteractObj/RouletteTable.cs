using UnityEngine;

public class RouletteTable : MonoBehaviour
{
    [SerializeField] private string rotateParam = "Rotate";
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Rotate()
    {
        animator.SetTrigger(rotateParam);
    }
}
