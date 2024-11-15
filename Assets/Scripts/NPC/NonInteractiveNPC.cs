using UnityEngine;

public class NonInteractiveNPC : MonoBehaviour
{
    [SerializeField] private bool haveRifle = false;
    [SerializeField] private string rifleLayer = "Rifle";
    private Animator animator;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        if (haveRifle)
        {
            int rifleIndex = animator.GetLayerIndex(rifleLayer);
            animator.SetLayerWeight(rifleIndex, 1f);
        }
    }

}
