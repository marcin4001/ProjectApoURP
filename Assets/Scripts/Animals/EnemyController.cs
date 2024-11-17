using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private bool isMoving;
    [SerializeField] private bool isDebug;
    [SerializeField] private Transform debugTarget;
    private Vector3 target;
    private NavMeshAgent agent;
    private EnemyAnim anim;
    private Coroutine coroutine;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<EnemyAnim>();
    }

    
    public void StartTurn()
    {
        Vector3 playerPos = FindFirstObjectByType<PlayerController>().transform.position;
        float distance = Vector3.Distance(transform.position, playerPos);
        if(distance > 0.7f)
        {
            MoveTo(playerPos);
        }
        else
        {
            Attack();
        }
    }

    public void MoveTo(Vector3 _target)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        target = _target;
        coroutine = StartCoroutine(Moving());
    }

    public void Attack()
    {
        Debug.Log("Attack");
        CombatController.instance.NextTurn();
    }

    public IEnumerator Moving()
    {
        agent.isStopped = false;
        float distance = Vector3.Distance(transform.position, target);
        agent.SetDestination(target);
        isMoving = true;
        anim.SetWalk(true);
        while (distance > 0.7f)
        {
            distance = Vector3.Distance(transform.position, target);
            yield return null;
        }
        isMoving = false;
        anim.SetWalk(false);
        agent.isStopped = true;
        CombatController.instance.NextTurn();
    }

    public void GetDamage()
    {
        Debug.Log("GetDamage");
    }

    void Update()
    {
        if(isDebug)
        {
            if(Input.GetKeyDown(KeyCode.F1))
            {
                MoveTo(debugTarget.position);
            }
        }
    }
}
