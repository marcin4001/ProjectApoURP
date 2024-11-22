using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private string nameEnemy;
    [SerializeField] private bool isMoving = false;
    [SerializeField] private bool isDeath = false;
    [SerializeField] private bool isDebug;
    [SerializeField] private Transform debugTarget;
    [SerializeField] private int damage = 5;
    [SerializeField] private int healthPoint = 10;
    [SerializeField] private GameObject bloodPrefab;
    [SerializeField] private EnemyGroup group;
    [SerializeField] private List<SlotItem> slots = new List<SlotItem>();
    private Vector3 target;
    private NavMeshAgent agent;
    private EnemyAnim anim;
    private Coroutine coroutine;
    private PlayerController player;
    private AudioSource source;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<EnemyAnim>();
        player = FindFirstObjectByType<PlayerController>();
        source = GetComponent<AudioSource>();
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
        if(source != null)
            source.Play();
        yield return new WaitForSeconds(0.5f);
        PlayerStats.instance.RemoveHealthPoint(damage);
        if(!PlayerStats.instance.isDeath())
            player.TakeDamage();
        else
            player.SetDeathState();
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
        HUDController.instance.AddConsolelog($"{nameEnemy} got hit! Loses");
        HUDController.instance.AddConsolelog($"{point} point(s).");
        if (!GameParam.instance.inCombat)
        {
            StartCoroutine(TriggerCombat());
        }
        if(healthPoint <= 0)
        {
            HUDController.instance.AddConsolelog($"{nameEnemy} dies.");
            healthPoint = 0;
            isDeath = true;
            anim.SetDeath();
            Destroy(agent);
            StartCoroutine(SpawnBlood());
            gameObject.AddComponent<EnemyInventory>();
        }
    }

    private IEnumerator TriggerCombat()
    {
        yield return new WaitForSeconds(1f);
        CombatController.instance.SetGroup(group);
        CombatController.instance.StartCombat(false);
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

    public List<SlotItem> GetItems()
    {
        return slots;
    }

    public string GetNameEnemy()
    {
        return nameEnemy;
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
