using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private string nameEnemy;
    [SerializeField] private int idEnemy = -1;
    [SerializeField] private bool isHuman = false;
    [SerializeField] private bool isHumanoid = false;
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
    [SerializeField] private OutlineList outlineList;
    [SerializeField] private Vector3 bloodOffset = Vector3.zero;
    [SerializeField] private EnemyGroup group;
    [SerializeField] private WeaponObject weapon;
    [SerializeField] private WeaponType weaponType = WeaponType.Rifle;
    [SerializeField] private List<SlotItem> slots = new List<SlotItem>();
    [SerializeField] private Transform nearPoint;
    [SerializeField] private int exp = 30;
    private Vector3 target;
    private NavMeshAgent agent;
    private EnemyAnim anim;
    private AnimationPlayer animHuman;
    private Coroutine coroutine;
    private PlayerController player;
    private AudioSource source;
    private NavMeshObstacle obstacle;

    private void Awake()
    {
        if (KilledEnemiesList.instance != null) 
        {
            if(KilledEnemiesList.instance.OnList(idEnemy))
            {
                Destroy(gameObject);
            }
        }
    }
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if(isHumanoid)
            isHuman = false;
        if(!isHuman)
            anim = GetComponentInChildren<EnemyAnim>();
        else
            animHuman = GetComponentInChildren<AnimationPlayer>();
        player = FindFirstObjectByType<PlayerController>();
        source = GetComponent<AudioSource>();
        obstacle = GetComponent<NavMeshObstacle>();
        if(outline != null)
            outline.enabled = false;
        if(outlineList != null)
            outlineList.Show(false);
        
        SetActiveObstacle(false);

        if(isHuman)
        {
            if(weaponType == WeaponType.Rifle)
                animHuman.ActiveRifleLayer();
            if(weaponType == WeaponType.HandGun)
                animHuman.ActiveHandGunLayer();
            if(weaponType == WeaponType.Melee)
                animHuman.ActiveBaseLayer();
            GetComponent<BoxCollider>().enabled = false;
        }

        if(isHumanoid)
            GetComponent<BoxCollider>().enabled = false;

        StartCoroutine(CorrectPositionYAfterTime());
    }


    private IEnumerator CorrectPositionYAfterTime()
    {
        yield return new WaitForEndOfFrame();
        CorrectPositionY();
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
            if (!CameraMovement.instance.ObjectInFov(transform))
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
        if (CombatController.instance.IsSkipTurnPlayer())
        {
            CombatController.instance.UnsetSkipTurnPlayer();
            yield return new WaitForSeconds(0.5f);
        }
        else
        {
            yield return new WaitForSeconds(1.5f);
        }
        if (outline != null)
            outline.enabled = true;
        if (outlineList != null)
            outlineList.Show(true);
        RotationToPlayer();
        if (!isHuman)
        {
            anim.Attack();
        }
        else
        {
            if (weaponType != WeaponType.Melee)
            {
                animHuman.Shot();
                if (weapon != null)
                {
                    weapon.StartPlayMuzzle();
                    weapon.StartPlayAttack();
                }
            }
            else
            {
                animHuman.Attack();
                if(weapon != null)
                    weapon.StartPlayAttack();
            }
        }
        if(source != null)
            source.Play();
        if (!isHuman)
        {
            yield return new WaitForSeconds(0.5f);
        }
        else
        {
            if(weaponType == WeaponType.Melee)
                yield return new WaitForSeconds(0.2f);
            else
                yield return new WaitForSeconds(0.5f);
        }
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
        yield return new WaitForSeconds(0.5f);
        CombatController.instance.NextTurn();
        if (outline != null)
            outline.enabled = false;
        if (outlineList != null)
            outlineList.Show(false);
    }

    public IEnumerator Moving()
    {
        CombatController.instance.UnsetSkipTurnPlayer();
        while(CombatController.instance.IsGetDamge())
            yield return new WaitForEndOfFrame();
        if (outline != null)
            outline.enabled = true;
        if (outlineList != null)
            outlineList.Show(true);
        agent.isStopped = false;
        float distance = Vector3.Distance(transform.position, target);
        agent.SetDestination(target);
        isMoving = true;
        if (!isHuman)
            anim.SetWalk(true);
        else
            animHuman.SetSpeedLocomotion(1f);
        while (distance > minDistance)
        {
            CorrectPositionY();
            //Debug.Log($"Name: {name} Time: {timeMoving}");
            if(timeMoving >= timeMaxMoving)
                break;
            distance = Vector3.Distance(transform.position, target);
            yield return new WaitForEndOfFrame();
            timeMoving += Time.deltaTime;
        }
        isMoving = false;
        if(!isHuman)
            anim.SetWalk(false);
        else
            animHuman.SetSpeedLocomotion(0f);
        agent.isStopped = true;
        RotationToPlayer();
        CombatController.instance.NextTurn();
        if (outline != null)
            outline.enabled = false;
        if (outlineList != null)
            outlineList.Show(false);
    }

    private void CorrectPositionY()
    {
        Transform meshPos = null;
        if (isHuman)
            meshPos = animHuman.transform;
        else
            meshPos = anim.transform;
        Vector3 newPosition = meshPos.position;
        newPosition.y = (isHuman) ? 0.01f : 0f;
        if(isHumanoid)
            newPosition.y = -0.07f;
        meshPos.position = newPosition;
    }

    public void GetDamage(int point, bool rawDamage = false, bool meleeWeapon = false)
    {
        bool isCrit = false;
        int pointDamage = CombatController.instance.CalculateDamegePlayer(point, out isCrit);
        if(rawDamage)
            pointDamage = CombatController.instance.CalculateDamegePlayerOnlyCrit(point, out isCrit);
        if(meleeWeapon)
            pointDamage = CombatController.instance.CalculateDamegePlayerMelee(point, out isCrit);
        healthPoint -= pointDamage;
        if(pointDamage <= 0)
        {
            HUDController.instance.AddConsolelog($"Thomas missed.");
        }
        else
        {
            if(isCrit)
            {
                HUDController.instance.AddConsolelog($"CRITICAL HIT! {nameEnemy}");
                HUDController.instance.AddConsolelog($"loses {pointDamage} point(s).");
            }
            else
            {
                HUDController.instance.AddConsolelog($"{nameEnemy} got hit! Loses");
                HUDController.instance.AddConsolelog($"{pointDamage} point(s).");
            }
        }
        
        if (!GameParam.instance.inCombat)
        {
            StartCoroutine(TriggerCombat());
        }
        if(healthPoint <= 0)
        {
            HUDController.instance.AddConsolelog($"{nameEnemy} dies.");
            GameParam.instance.AddExp(exp);
            healthPoint = 0;
            if(!isDeath && KilledEnemiesList.instance != null)
            {
                KilledEnemiesList.instance.AddToList(idEnemy);
            }
            isDeath = true;
            if(!isHuman)
                anim.SetDeath();
            else
                animHuman.SetDeath();
            Destroy(agent);
            StartCoroutine(SpawnBlood());
            gameObject.AddComponent<EnemyInventory>();
            if(isHuman || isHumanoid)
            {
                GetComponent<BoxCollider>().enabled = true;
                GetComponent<CapsuleCollider>().enabled = false;
            }

        }
        if(healthPoint > 0 && isHuman)
        {
            StartCoroutine(GetDamageAnim());
        }
        if (healthPoint > 0 && isHumanoid)
        {
            StartCoroutine(GetDamageAnimHumanoid());
        }
    }

    private IEnumerator GetDamageAnim()
    {
        CombatController.instance.SetGetDamage(true);
        yield return new WaitForSeconds(0.2f);
        animHuman.TakeDamage();
        yield return new WaitForSeconds(0.5f);
        CombatController.instance.SetGetDamage(false);
    }
    private IEnumerator GetDamageAnimHumanoid()
    {
        CombatController.instance.SetGetDamage(true);
        yield return new WaitForSeconds(0.2f);
        anim.TakeDamage();
        yield return new WaitForSeconds(0.5f);
        CombatController.instance.SetGetDamage(false);
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
        Vector3 bloodPos = Vector3.one;
        if (isHuman || isHumanoid)
        {
            bloodPos = transform.position - transform.forward * 1f;
            bloodPos.y = 0.01f;
        }
        else
        {
            bloodPos = blood.transform.position + bloodOffset;
            bloodPos.y = 0;
        }
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

    public Vector3 GetNearPoint()
    {
        if(nearPoint != null)
            return nearPoint.position;
        return transform.position;
    }

    public void RandomizeID()
    {
        idEnemy = Random.Range(0, 10000);
    }

    //void Update()
    //{
    //    if(isDebug)
    //    {
    //        if(Input.GetKeyDown(KeyCode.F1))
    //        {
    //            MoveTo(debugTarget.position);
    //        }
    //    }
    //}
}
