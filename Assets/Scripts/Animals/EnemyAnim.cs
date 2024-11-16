using UnityEngine;

public class EnemyAnim : MonoBehaviour
{
    [SerializeField] private string walkParam = "Walk";
    [SerializeField] private string deathParam = "Death";
    [SerializeField] private bool isWalk = false;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        isWalk = false;
    }

    public void SetWalk(bool _isWalk)
    {
        isWalk = _isWalk;
        animator.SetBool(walkParam, isWalk);
    }

    public void SetDeath()
    {
        animator.SetTrigger(deathParam);
    }
}
