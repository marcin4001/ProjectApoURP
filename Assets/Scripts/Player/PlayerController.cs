using System.Collections;
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
    [SerializeField] private bool inMenu = false;
    [SerializeField] private Transform center;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float maxDistance = 2f;
    [SerializeField] private float radiusPlayerCutWall = 3.5f;
    [SerializeField] private Transform handSocket;
    [SerializeField] private GameObject itemInHand;
    [SerializeField] private Transform debugObject;
    [SerializeField] private bool block;
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
    void Awake()
    {
        inputSystem = new MainInputSystem();
        inputSystem.Enable();
        inputSystem.Player.MousePos.performed += SetMousePosition;
        inputSystem.Player.PlayerAction.performed += PlayerAction;
        inputSystem.Player.ChangeStateAction.performed += ChangeActionState;
        weaponController = GetComponent<WeaponController>();
        animationPlayer = GetComponentInChildren<AnimationPlayer>();
        playerStats = GetComponent<PlayerStats>();
    }

    void OnEnable()
    {
        inputSystem.Enable();
        inputSystem.Player.MousePos.performed += SetMousePosition;
        inputSystem.Player.PlayerAction.performed += PlayerAction;
        inputSystem.Player.ChangeStateAction.performed += ChangeActionState;
    }

    void OnDisable()
    {
        inputSystem.Player.PlayerAction.performed -= PlayerAction;
        inputSystem.Player.MousePos.performed -= SetMousePosition;
        inputSystem.Player.ChangeStateAction.performed -= ChangeActionState;
        inputSystem.Disable();
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
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
                UseItem(hit.point);
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
    }


    private IEnumerator Drinking(Item item)
    {
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

    public void UseItem(Vector3 point)
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
                weapon.StartPlayMuzzle();
        }
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
        currentCoroutine = StartCoroutine(MoveTask());
    }

    private void Use(GameObject obj)
    {
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
                    ObjectInfoLog logObjTemp = hit.collider.GetComponent<ObjectInfoLog>();
                    if (logObjTemp != null && distance < radiusPlayerCutWall)
                        logObj = logObjTemp;
                }
            }
            string log = logObj.GetLog();
            HUDController.instance.AddConsolelog(log);
        }
    }


    private IEnumerator MoveTask()
    {
        isMoving = true;
        agent.SetDestination(moveTarget);
        while(agent.pathPending)
            yield return new WaitForEndOfFrame();

        currentPath = agent.path;
        indexCorner = 1;

        while (agent.remainingDistance > 0f)
        {
            float distance = Vector3.Distance(transform.position, currentPath.corners[indexCorner]);
            if (distance < 0.5f && currentPath.corners.Length - 1 > indexCorner)
                indexCorner++;
            Quaternion targetRotation = Quaternion.LookRotation(currentPath.corners[indexCorner] - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            animationPlayer.SetSpeedLocomotion(agent.velocity.magnitude);
            yield return new WaitForEndOfFrame();
            if ((currentSelectObj is BackgroundNPC || currentSelectObj is DialogueNPC) && agent.remainingDistance < 0.5f)
                break;
        }
        agent.isStopped = true;
        animationPlayer.SetSpeedLocomotion(0f);
        isMoving = false;
        if(currentSelectObj != null)
        {
            float distnceToObject = Vector3.Distance(transform.position, moveTarget);
            if(distnceToObject < maxDistance)
                StartCoroutine(InteractAction(currentSelectObj));
            currentSelectObj = null;
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
        isUsingObj = true;
        weaponController.ShowCurrentWeapon(false);
        if(usable is Door)
        {
            Door door = (Door)usable;
            Vector3 slot = door.GetNearPoint();
            agent.Warp(slot);
            transform.rotation = Quaternion.LookRotation(door.GetRootPosition() - transform.position);
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
        isUsingObj = false;
    }

    public Vector3 GetCenterPosition()
    {
        return center.position;
    }

}

public enum PlayerActionState
{
    Move, Use, Look
}
