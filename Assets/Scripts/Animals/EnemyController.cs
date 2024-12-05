using System.Collections;
using System.Collections.Generic;
using System.Drawing;
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
    [SerializeField] private int chanceToHit = 100;
    [SerializeField] private int chanceToCrit = 20;
    [SerializeField] private int healthPoint = 10;
    [SerializeField] private float minDistance = 0.7f;
    [SerializeField] private int indexSlot = 0;
    [SerializeField] private float timeMaxMoving = 30f;
    [SerializeField] private float timeMoving = 0f;
    [SerializeField] private GameObject bloodPrefab;
    [SerializeField] private Outline outline;
    [SerializeField] private Vector3 bloodOffset = Vector3.zero;
    [SerializeField] private EnemyGroup group;
    [SerializeField] private List<SlotItem> slots = new List<SlotItem>();
    private Vector3 target;
    private NavMeshAgent agent;
    private EnemyAnim anim;
    private Coroutine coroutine;
    private PlayerController player;
    private AudioSource source;
    private NavMeshObstacle obstacle;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<EnemyAnim>();
        player = FindFirstObjectByType<PlayerController>();
        source = GetComponent<AudioSource>();
        obstacle = GetComponent<NavMeshObstacle>();
        if(outline != null)
            outline.enabled = false;
        SetActiveObstacle(false);
    }

    
    public void StartTurn()
    {
        if (isDeath)
        {
            CombatController.instance.NextTurn();
            return;
        }
        
        Vector3 playerPos = CombatController.instance.GetSlot(indexSlot);
        float distance = Vector3.Distance(transform.position, playerPos);
        timeMoving = 0f;
        if(distance > minDistance)
        {
            if (distance > 2f)
                CameraMovement.instance.CenterCameraTo(transform);
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
        yield return new WaitForSeconds(0.75f);
        if (outline != null)
            outline.enabled = true;
        RotationToPlayer();
        anim.Attack();
        if(source != null)
            source.Play();
        yield return new WaitForSeconds(0.5f);
        bool isCrit = false;
        int _damage = CombatController.instance.CalculateDamege(damage, chanceToHit, chanceToCrit, out isCrit);
        if(_damage > 0)
        {
            if(isCrit)
            {
                HUDController.instance.AddConsolelog($"CRITICAL HIT! You lose {_damage}");
                HUDController.instance.AddConsolelog("point(s).");
            }
            else
            {
                HUDController.instance.AddConsolelog($"You're hit! You lose {_damage}");
                HUDController.instance.AddConsolelog("point(s).");
            }
        }
        else
        {
            HUDController.instance.AddConsolelog($"{nameEnemy} missed.");
        }
        
        PlayerStats.instance.RemoveHealthPoint(_damage);
        if (_damage > 0)
        {
            if (!PlayerStats.instance.isDeath())
                player.TakeDamage();
            else
                player.SetDeathState();
        }
        CombatController.instance.NextTurn();
        if (outline != null)
            outline.enabled = false;
    }

    public IEnumerator Moving()
    {
        if (outline != null)
            outline.enabled = true;
        agent.isStopped = false;
        float distance = Vector3.Distance(transform.position, target);
        agent.SetDestination(target);
        isMoving = true;
        anim.SetWalk(true);
        while (distance > minDistance)
        {
            Debug.Log($"Name: {name} Time: {timeMoving}");
            if(timeMoving >= timeMaxMoving)
            {
                //timeMoving = 0;
                break;
            }
            distance = Vector3.Distance(transform.position, target);
            yield return new WaitForEndOfFrame();
            timeMoving += Time.deltaTime;
        }
        isMoving = false;
        anim.SetWalk(false);
        agent.isStopped = true;
        RotationToPlayer();
        CombatController.instance.NextTurn();
        if (outline != null)
            outline.enabled = false;
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
        Vector3 bloodPos = blood.transform.position + bloodOffset;
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

    public void SetPriority(int priority)
    {
        if (agent == null)
            return;
        agent.avoidancePriority = priority;
    }

    public void SetActiveObstacle(bool activeObstacle)
    {
        if(obstacle == null)
            return;
        obstacle.enabled = activeObstacle;
        agent.enabled = !activeObstacle;
    }

    public void SetActiveAgent(bool activeAgent)
    {
        if(agent != null)
            agent.enabled = activeAgent;
    }

    public void SetIndexSlot(int index)
    {
        indexSlot = index;
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
