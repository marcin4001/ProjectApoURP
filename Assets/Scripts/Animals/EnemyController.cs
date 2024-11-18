using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private bool isMoving = false;
    [SerializeField] private bool isDeath = false;
    [SerializeField] private bool isDebug;
    [SerializeField] private Transform debugTarget;
    [SerializeField] private int healthPoint = 10;
    [SerializeField] private GameObject bloodPrefab;
    private Vector3 target;
    private NavMeshAgent agent;
    private EnemyAnim anim;
    private Coroutine coroutine;
    private PlayerController player;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<EnemyAnim>();
        player = FindFirstObjectByType<PlayerController>();
    }

    
    public void StartTurn()
    {
        if (isDeath)
        {
            CombatController.instance.NextTurn();
            return;
        }
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
        coroutine = StartCoroutine(Attacking());
    }

    public IEnumerator Attacking()
    {
        yield return new WaitForSeconds(1.5f);
        RotationToPlayer();
        anim.Attack();
        yield return new WaitForSeconds(0.5f);
        PlayerStats.instance.RemoveHealthPoint(5);
        player.TakeDamage();
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
        RotationToPlayer();
        CombatController.instance.NextTurn();
    }

    public void GetDamage(int point)
    {
        healthPoint -= point;
        if(healthPoint <= 0)
        {
            healthPoint = 0;
            isDeath = true;
            anim.SetDeath();
            Destroy(agent);
            StartCoroutine(SpawnBlood());
        }
    }

    private IEnumerator SpawnBlood()
    {
        yield return new WaitForSeconds(1f);
        GameObject blood = Instantiate(bloodPrefab, transform.position, Quaternion.identity);
        Vector3 bloodPos = blood.transform.position;
        bloodPos.y = 0;
        blood.transform.position = bloodPos;
    }

    private void RotationToPlayer()
    {
        Vector3 playerPos = FindFirstObjectByType<PlayerController>().transform.position;
        Vector3 direction = new Vector3(playerPos.x - transform.position.x, 0f, playerPos.z - transform.position.z);
        if (direction != null)
            transform.rotation = Quaternion.LookRotation(direction);
    }

    public bool IsDeath()
    {
        return isDeath;
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
