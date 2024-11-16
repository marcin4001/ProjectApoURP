using System.Collections;
using UnityEngine;
using UnityEngine.AI;

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

    
    public void MoveTo(Vector3 _target)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        target = _target;
        coroutine = StartCoroutine(Moving());
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
