using System.Collections;
using System.Drawing;
using UnityEngine;
using UnityEngine.AI;

public class DogPatrol : MonoBehaviour
{
    [SerializeField] private float range = 4f;
    [SerializeField] private float waitingTime = 2f;
    [SerializeField] private EnemyAnim anim;
    [SerializeField] private float counter = 0f;
    [SerializeField] private float patrolTime = 5f;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(Patrol());
    }


    public Vector3 GetRandomWaypoint()
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 point = transform.position + Random.insideUnitSphere * range;
            point.y = transform.position.y;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(point, out hit, 1f, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }
        return transform.position;
    }

    public IEnumerator Patrol()
    {
        yield return new WaitForEndOfFrame();
        Vector3 target = GetRandomWaypoint();
        float distance = Vector3.Distance(transform.position, target);
        agent.SetDestination(target);
        anim.SetWalk(true);
        while (true)
        {
            while(distance > 0.7f)
            {
                yield return new WaitForEndOfFrame();
                counter += Time.deltaTime;
                if(counter >= patrolTime) break;
                distance = Vector3.Distance(transform.position, target);
            }
            agent.isStopped = true;
            counter = 0;
            anim.SetWalk(false);
            yield return new WaitForSeconds(waitingTime);
            agent.isStopped = false;
            anim.SetWalk(true);
            target = GetRandomWaypoint();
            agent.SetDestination(target);
            distance = Vector3.Distance(transform.position, target);
        }
        
    }
}
