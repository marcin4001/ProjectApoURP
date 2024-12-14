using UnityEngine;

public class EnemyAnim : MonoBehaviour
{
    [SerializeField] private string walkParam = "Walk";
    [SerializeField] private string deathParam = "Death";
    [SerializeField] private string attackParam = "Attack";
    [SerializeField] private string takeDamageParam = "TakeDamage";
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

    public void Attack()
    {
        animator.SetTrigger(attackParam);
    }

    public void SetDeath()
    {
        animator.SetTrigger(deathParam);
    }

    public void TakeDamage()
    {
        animator.SetTrigger(takeDamageParam);
    }
}
