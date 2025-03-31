using System.Collections;
using NUnit.Framework.Internal;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private LayerMask layerMove;
    [SerializeField] private LayerMask layerUseLook;
    [SerializeField] private LayerMask layerInsideHouse;
    [SerializeField] private PlayerActionState actionState = PlayerActionState.Move;
    [SerializeField] private bool isUsingItem = false;
    [SerializeField] private bool isMoving = false;
    [SerializeField] private bool isUsingObj = false;
    [SerializeField] private bool isUsingKey = false;
    [SerializeField] private bool inMenu = false;
    [SerializeField] private Transform center;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float maxDistance = 2f;
    [SerializeField] private float radiusPlayerCutWall = 3.5f;
    [SerializeField] private Transform handSocket;
    [SerializeField] private GameObject itemInHand;
    [SerializeField] private Transform debugObject;
    [SerializeField] private bool block;
    [SerializeField] private bool stopFlag = false;
    [SerializeField] private float counterMoving = 0;
    [SerializeField] private float counterMovingMax = 5;
    private int keyID = 0;
    private int indexCorner = 1;
    private LayerMask layer;
    private Vector2 mousePosition;
    private NavMeshAgent agent;
    private NavMeshPath currentPath;
    private MainInputSystem inputSystem;
    private Camera camera;
    private AnimationPlayer animationPlayer;
    private WeaponController weaponController;
    private PlayerStats playerStats;

    private Coroutine currentCoroutine;
    private IUsableObj currentSelectObj;
    private Vector3 moveTarget;
    private Vector3 lastPos;
    private float blockTime = 0;
    void Awake()
    {
        inputSystem = new MainInputSystem();
        inputSystem.Enable();
        inputSystem.Player.MousePos.performed += SetMousePosition;
        inputSystem.Player.PlayerAction.performed += PlayerAction;
        inputSystem.Player.ChangeStateAction.performed += ChangeActionState;
        inputSystem.Player.Test.performed += TestClick;
        weaponController = GetComponent<WeaponController>();
        animationPlayer = GetComponentInChildren<AnimationPlayer>();
        playerStats = GetComponent<PlayerStats>();
        agent = GetComponent<NavMeshAgent>();
    }

    void OnEnable()
    {
        inputSystem.Enable();
        inputSystem.Player.MousePos.performed += SetMousePosition;
        inputSystem.Player.PlayerAction.performed += PlayerAction;
        inputSystem.Player.ChangeStateAction.performed += ChangeActionState;
        inputSystem.Player.Test.performed += TestClick;
    }

    void OnDisable()
    {
        inputSystem.Player.PlayerAction.performed -= PlayerAction;
        inputSystem.Player.MousePos.performed -= SetMousePosition;
        inputSystem.Player.ChangeStateAction.performed -= ChangeActionState;
        inputSystem.Player.Test.performed -= TestClick;
        inputSystem.Disable();
    }

    void Start()
    {
        camera = FindFirstObjectByType<Camera>();
        
        HUDController.instance.SetStateButtons(actionState);
        layer = layerMove;
        currentPath = new NavMeshPath();

    }

    private void SetMousePosition(InputAction.CallbackContext ctx)
    {
        mousePosition = ctx.ReadValue<Vector2>();
    }

    private void PlayerAction(InputAction.CallbackContext ctx)
    {
        if (block || inMenu)
            return;
        if (HUDController.instance.PointerOnHUD())
            return;
        Ray ray = camera.ScreenPointToRay(mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 1000f, layer))
        {
            if(isUsingItem)
            {
                UseItem(hit.point, hit.collider.gameObject);
                isUsingItem = false;
                return;
            }
            switch(actionState)
            {
                case PlayerActionState.Move:
                    MovePlayer(hit.point);
                    break;
                case PlayerActionState.Use:
                    Use(hit.collider.gameObject);
                    break;
                case PlayerActionState.Look:
                    LookObject(hit.collider.gameObject);
                    break;
            }
        }
    }

    public void StartUsingItem()
    {
        if (block || isMoving)
            return;
        isUsingItem = true;
    }

    public void Eat(Item item)
    {
        if (block)
            return;
        FoodItem foodItem = item as FoodItem;
        if(foodItem.isDrink)
            StartCoroutine(Drinking(item));
        else
            StartCoroutine(Eating(item));
    }

    private IEnumerator Eating(Item item)
    {
        HUDController.instance.SetActiveInventoryBtn(false);
        CursorController.instance.SetIsWait(true);
        animationPlayer.Eating();
        float time = animationPlayer.GetEatingTime();
        yield return new WaitForSeconds(time);
        FoodItem food = (FoodItem) item;
        playerStats.AddHealthPoint(food.healPoint);
        if (food.radioactive)
            playerStats.AddOneRadLevel();
        if(food.healRadioactive) 
            playerStats.RemoveOneRadLevel();
        HUDController.instance.RemoveCurrentItem();
        HUDController.instance.SetActiveInventoryBtn(true);
        CursorController.instance.SetIsWait(false);
        if (GameParam.instance.inCombat)
        {
            CombatController.instance.RemoveAP(1);
        }
    }


    private IEnumerator Drinking(Item item)
    {
        HUDController.instance.SetActiveInventoryBtn(false);
        CursorController.instance.SetIsWait(true);
        animationPlayer.Drink();
        float time = animationPlayer.GetDrinkingTime();
        yield return new WaitForSeconds(time);
        FoodItem food = (FoodItem)item;
        playerStats.AddHealthPoint(food.healPoint);
        if (food.radioactive)
            playerStats.AddOneRadLevel();
        if (food.healRadioactive)
            playerStats.RemoveOneRadLevel();
        HUDController.instance.RemoveCurrentItem();
        HUDController.instance.SetActiveInventoryBtn(true);
        CursorController.instance.SetIsWait(false);
        if (GameParam.instance.inCombat)
        {
            CombatController.instance.RemoveAP(1);
        }
    }

    public void ShowWeapon(Item weapon)
    {
        if(weapon == null)
        {
            weaponController.SetEmptyCurrentWeapon();
            animationPlayer.ActiveBaseLayer();
            return;
        }
        weaponController.ShowWeapon(weapon.id);
        weaponController.SetCurrentWeapon(weapon);
        WeaponItem weaponItem = (WeaponItem) weapon;
        switch(weaponItem.type)
        {
            case WeaponType.Rifle:
                animationPlayer.ActiveRifleLayer();
                break;
            case WeaponType.HandGun: 
                animationPlayer.ActiveHandGunLayer();
                break;
            case WeaponType.Melee:
                animationPlayer.ActiveBaseLayer();
                break;
            default:
                animationPlayer.ActiveBaseLayer();
                break;
        }
    }

    public void SpawnItemInHand(Item item)
    {
        if(item is FoodItem)
        {
            FoodItem foodItem = (FoodItem) item;
            itemInHand = Instantiate(foodItem.spawnObj, handSocket);
        }
    }

    public void RemoveItemInHand()
    {
        if(itemInHand != null)
            Destroy(itemInHand);
    }

    public void UseItem(Vector3 point, GameObject _target)
    {
        SlotItem slotItem = HUDController.instance.GetCurrentItem();
        Item item = slotItem.GetItem();
        if (item == null)
            return;
        if (item is WeaponItem)
        {
            WeaponObject weapon = weaponController.GetCurrentWeapon();
            if (weapon == null)
                return;
            EnemyController enemy = _target.GetComponent<EnemyController>();
            if (enemy == null) return;
            if (enemy.IsDeath()) return;
            float distanceToPoint = Vector3.Distance(center.position, point);
            if(distanceToPoint > weapon.GetRange())
            {
                HUDController.instance.AddConsolelog("The target is too far");
                HUDController.instance.AddConsolelog("away.");
                return;
            }
            Vector3 direction = point - center.position;
            RaycastHit[] hits = Physics.RaycastAll(center.position, direction, distanceToPoint);
            foreach (RaycastHit hit in hits)
            {
                if(hit.collider.tag == "Wall" || hit.collider.tag == "Door")
                {
                    HUDController.instance.AddConsolelog("The target is too far");
                    HUDController.instance.AddConsolelog("away.");
                    return;
                }
            }

            if (weapon.IsMelee())
            {
                animationPlayer.Attack();
            }
            else
            {
                if (weapon.OutOfAmmo())
                    return;
                animationPlayer.Shot();
                weapon.RemoveAmmo(1);
            }
            transform.rotation = Quaternion.LookRotation(point - transform.position);
            if (weapon != null)
            {
                weapon.StartPlayMuzzle();
                weapon.StartPlayAttack();
            }
            enemy.GetDamage(weapon.GetDamage());
            if (GameParam.instance.inCombat)
            {
                StartCoroutine(AfterUseWeaponInCombat());
            }
            
        }
        if(item is MiscItem)
        {
            Debug.Log("Misc");
            Door door = _target.GetComponent<Door>();
            if(door != null)
            {
                Debug.Log("Door");
                if (!door.IsLock())
                {
                    HUDController.instance.AddConsolelog("These doors are not locked.");
                    return;
                }
                if (currentCoroutine != null)
                    StopCoroutine(currentCoroutine);
                float distnceToObject = Vector3.Distance(transform.position, _target.transform.position);
                if (distnceToObject > maxDistance)
                {
                    currentSelectObj = door;
                    agent.isStopped = false;
                    moveTarget = currentSelectObj.GetNearPoint();
                    currentCoroutine = StartCoroutine(MoveTask());
                    isUsingKey = true;
                    keyID = item.id;
                    return;
                }
                agent.isStopped = true;
                animationPlayer.SetSpeedLocomotion(0f);
                isUsingKey = true;
                keyID = item.id;
                StartCoroutine(InteractAction(door));
            }
        }
    }

    private IEnumerator AfterUseWeaponInCombat()
    {
        SetBlock(true);
        yield return new WaitForSeconds(0.3f);
        CombatController.instance.RemoveAP(2);
    }

    public void ReloadGun()
    {
        if (block || isMoving)
            return;
        WeaponObject weapon = weaponController.GetCurrentWeapon();
        if (weapon == null)
            return;
        if (weapon.IsFull())
            return;
        animationPlayer.Reload();
        weapon.Reload();
        weapon.PlayReloadSound();
    }

    public PlayerActionState GetState()
    {
        return actionState;
    }

    public bool IsUsingItem()
    {
        return isUsingItem;
    }

    public bool InMenu()
    {
        return inMenu;
    }

    public float GetRadiusPlayerCutWall()
    {
        return radiusPlayerCutWall;
    }

    public void SetState(PlayerActionState newState)
    {
        actionState = newState;
        layer = layerUseLook;
        if(actionState == PlayerActionState.Move)
        {
            layer = layerMove;
        }
    }

    public void SetInMenu(bool value)
    {
        inMenu = value;
    }

    public bool IsBlock()
    {
        return block;
    }

    public void SetBlock(bool value)
    {
        block = value;
        CursorController.instance.SetIsWait(value);
    }

    public Vector2 GetMousePosition()
    {
        return mousePosition;
    }

    private void ChangeActionState(InputAction.CallbackContext ctx)
    {
        if (block || inMenu)
            return;
        if (mousePosition.x < 0 || mousePosition.x > Screen.width || mousePosition.y < 0 || mousePosition.y > Screen.height)
            return;
        if(isUsingItem)
        {
            isUsingItem = false;
            return;
        }
        if (HUDController.instance.PointerOnHUD())
            return;
        layer = layerUseLook;
        switch(actionState)
        {
            case PlayerActionState.Move:
                actionState = PlayerActionState.Use;
                break;
            case PlayerActionState.Use:
                actionState = PlayerActionState.Look;
                break;
            case PlayerActionState.Look:
                actionState = PlayerActionState.Move;
                layer = layerMove;
                break;
        }
        HUDController.instance.SetStateButtons(actionState);
    }

    public void TestClick(InputAction.CallbackContext ctx)
    {
        StopMove();
    }

    public Vector3 RoundPosition(Vector3 position)
    {
        return new Vector3(Mathf.Round(position.x), position.y, Mathf.Round(position.z));
    }

    private void MovePlayer(Vector3 point)
    {
        if (isUsingObj)
            return;
        float distanceToPlayer = Vector3.Distance(transform.position, point);
        if (distanceToPlayer < 0.5f)
            return;
        NavMeshHit navMeshHit;
        if (!NavMesh.SamplePosition(point, out navMeshHit, 0.5f, NavMesh.AllAreas))
            return;
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);
        agent.isStopped = false;
        moveTarget = point;
        currentSelectObj = null;
        currentCoroutine = StartCoroutine(MoveTask());
    }

    private void Use(GameObject obj)
    {
        if (GameParam.instance.inCombat)
        {
            HUDController.instance.AddConsolelog("Cannot be used during");
            HUDController.instance.AddConsolelog("combat");
            return;
        }
        if (isUsingObj)
            return;
        IUsableObj usable = obj.GetComponent<IUsableObj>();

        if (usable == null)
        {
            if(obj.layer == 10)
            {
                Ray ray = camera.ScreenPointToRay(mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 1000f, layerInsideHouse))
                {
                    if(debugObject != null)
                        debugObject.position = hit.point;
                    Vector3 playerScreenPosition = camera.WorldToScreenPoint(GetCenterPosition());
                    float distance = Vector3.Distance(playerScreenPosition, mousePosition);
                    IUsableObj usableTemp = hit.collider.GetComponent<IUsableObj>();
                    if (usableTemp != null && distance < radiusPlayerCutWall)
                    {
                        usable = usableTemp;
                        obj = hit.collider.gameObject;
                    }
                }
            }
        }
        if (usable == null)
            return;
        if(!usable.CanUse())
        {
            return;
        }
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);
        float distnceToObject = Vector3.Distance(transform.position, obj.transform.position);
        if(usable is BackgroundNPC || usable is DialogueNPC)
            distnceToObject = Vector3.Distance(transform.position, usable.GetNearPoint());
        if(distnceToObject > maxDistance)
        {
            currentSelectObj = usable;
            agent.isStopped = false;
            moveTarget = currentSelectObj.GetNearPoint();
            currentCoroutine = StartCoroutine(MoveTask());
            return;
        }
        agent.isStopped = true;
        animationPlayer.SetSpeedLocomotion(0f);
        StartCoroutine(InteractAction(usable));
    }

    private void LookObject(GameObject obj)
    {
        ObjectInfoLog logObj = obj.GetComponent<ObjectInfoLog>();
        if(logObj != null)
        {
            if(obj.layer == 10)
            {
                Ray ray = camera.ScreenPointToRay(mousePosition);
                RaycastHit hit;
                if(Physics.Raycast(ray, out hit, 1000f, layerInsideHouse))
                {
                    Vector3 playerScreenPosition = camera.WorldToScreenPoint(GetCenterPosition());
                    float distance = Vector3.Distance(playerScreenPosition, mousePosition);
                    ObjectInfoLogList logListTemp = hit.collider.GetComponent<ObjectInfoLogList>();
                    if(logListTemp != null)
                    {
                        logListTemp.ShowLogs();
                        return;
                    }
                    ObjectInfoLog logObjTemp = hit.collider.GetComponent<ObjectInfoLog>();
                    if (logObjTemp != null && distance < radiusPlayerCutWall)
                        logObj = logObjTemp;
                }
            }
            string log = logObj.GetLog();
            HUDController.instance.AddConsolelog(log);
        }
        ObjectInfoLogList logList = obj.GetComponent<ObjectInfoLogList>();
        if(logList != null)
        {
            logList.ShowLogs();
        }
    }


    private IEnumerator MoveTask()
    {
        if (GameParam.instance.inCombat)
        {
            counterMoving = 0;
            SetBlock(true);
        }
        isMoving = true;
        agent.SetDestination(moveTarget);
        while(agent.pathPending)
            yield return new WaitForEndOfFrame();
        //if(debugObject != null)
        //    debugObject.position = moveTarget;
        currentPath = agent.path;
        indexCorner = 1;
        lastPos = transform.position;
        while (agent.remainingDistance > 0f)
        {
            float distance = Vector3.Distance(transform.position, currentPath.corners[indexCorner]);
            if (distance < 0.5f && currentPath.corners.Length - 1 > indexCorner)
                indexCorner++;
            if (GameParam.instance.inCombat)
            {
                counterMoving += Time.deltaTime;
                Debug.Log($"Player moving time: {counterMoving}");
                if(counterMoving >= counterMovingMax)
                    break;
            }
            Quaternion targetRotation = Quaternion.LookRotation(currentPath.corners[indexCorner] - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            animationPlayer.SetSpeedLocomotion(agent.velocity.magnitude);
            yield return new WaitForEndOfFrame();
            if (currentSelectObj is BackgroundNPC && agent.remainingDistance < 0.7f)
                break;
            if (currentSelectObj is DialogueNPC && agent.remainingDistance < 0.7f)
                break;
            if(agent.remainingDistance < 0.7f && stopFlag)
                stopFlag = false;
            if(stopFlag)
            {
                stopFlag = false;
                break;
            }
            if(Vector3.Distance(transform.position, lastPos) < 0.005f)
            {
                blockTime += Time.deltaTime;
                //Debug.Log(blockTime);
                if(blockTime >= 1.5f)
                {
                    SetBlock(false);
                    blockTime = 0f;
                    Debug.Log("break");
                    break;
                }
            }
            else
            {
                blockTime = 0f;
            }
            lastPos = transform.position;
        }
        agent.isStopped = true;
        animationPlayer.SetSpeedLocomotion(0f);
        isMoving = false;
        if (GameParam.instance.inCombat)
        {
            CombatController.instance.RemoveAP(2);
        }
        if (currentSelectObj != null)
        {
            float distnceToObject = Vector3.Distance(transform.position, moveTarget);
            if(distnceToObject < maxDistance)
                StartCoroutine(InteractAction(currentSelectObj));
            if(isUsingKey)
                StartCoroutine(InteractAction(currentSelectObj));
            currentSelectObj = null;
        }
    }

    public void StopMove()
    {
        Debug.Log("Stop");
        if(isMoving)
        {
            stopFlag = true;
            //if(currentCoroutine != null)
            //{
            //    StopCoroutine(currentCoroutine);
            //    currentCoroutine = null;
            //}
            //isMoving = false;
            //agent.isStopped = true;
            //animationPlayer.SetSpeedLocomotion(0f);
            //currentSelectObj = null;
        }
    }

    public bool IsNearPlayer(Vector3 position, float maxDistance)
    {
        float distance = Vector3.Distance(position, transform.position);
        Debug.Log($"Distance: {distance}");
        return distance <= maxDistance;
    }

    public IEnumerator InteractAction(IUsableObj usable)
    {
        HUDController.instance.SetActiveInventoryBtn(false);
        CursorController.instance.SetIsWait(true);
        isUsingObj = true;
        weaponController.ShowCurrentWeapon(false);
        if(usable is Door)
        {
            Door door = (Door)usable;
            Vector3 slot = door.GetNearPoint();
            agent.Warp(slot);
            transform.rotation = Quaternion.LookRotation(door.GetRootPosition() - transform.position);
            if (isUsingKey)
            {
                if (door.CheckKey(keyID))
                {
                    door.Unlock();
                }
                else
                {
                    HUDController.instance.AddConsolelog("The key doesn't fit.");
                }
            }
        }
        else if(usable is EnemyInventory)
        {
            transform.rotation = Quaternion.LookRotation(usable.GetNearPoint() - transform.position);
        }
        else
        {
            GameObject usableObj = usable.GetMainGameObject();
            transform.rotation = Quaternion.LookRotation(usableObj.transform.position - transform.position);
        }
        if (usable is BackgroundNPC || usable is DialogueNPC)
        {
            weaponController.ShowCurrentWeapon(true);
            yield return null;
        }
        else
        {
            animationPlayer.DoorInteract();
            yield return new WaitForSeconds(animationPlayer.GetDoorInteractTime());
            weaponController.ShowCurrentWeapon(true);
        }
        usable.Use();
        if(usable is Door)
        {
            Door door = (Door)usable;
            if (door.IsLock() && !isUsingKey)
                HUDController.instance.AddConsolelog("The door is locked.");
        }
        isUsingObj = false;
        isUsingKey = false;
        HUDController.instance.SetActiveInventoryBtn(true);
        if(!(usable is Bed))
            CursorController.instance.SetIsWait(false);
    }

    public void TakeDamage()
    {
        animationPlayer.TakeDamage();
    }

    public void SetDeathState()
    {
        animationPlayer.SetDeath();    
    }

    public Vector3 GetCenterPosition()
    {
        return center.position;
    }

    public NavMeshAgent GetAgent()
    {
        return agent;
        
    }

    public AnimationPlayer GetAnim()
    {
        return animationPlayer;
    }

    public void PriorityUp()
    {
        agent.avoidancePriority = agent.avoidancePriority - 1;
    }

    public void PriorityDown()
    {
        agent.avoidancePriority = agent.avoidancePriority + 1;
    }
}

public enum PlayerActionState
{
    Move, Use, Look
}
