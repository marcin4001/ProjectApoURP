using System.Collections;
using System.Drawing;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private LayerMask layerMove;
    [SerializeField] private LayerMask layerUseLook;
    [SerializeField] private LayerMask layerInsideHouse;
    [SerializeField] private PlayerActionState actionState = PlayerActionState.Move;
    [SerializeField] private Transform center;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float maxDistance = 2f;
    [SerializeField] private float radiusPlayerCutWall = 3.5f;
    private int indexCorner = 1;
    private LayerMask layer;
    private Vector2 mousePosition;
    private NavMeshAgent agent;
    private NavMeshPath currentPath;
    private MainInputSystem inputSystem;
    private Camera camera;
    private AnimationPlayer animationPlayer;

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
        animationPlayer = GetComponentInChildren<AnimationPlayer>();
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
        if (HUDController.instance.PointerOnHUD())
            return;
        Ray ray = camera.ScreenPointToRay(mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 1000f, layer))
        {
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

    public PlayerActionState GetState()
    {
        return actionState;
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

    public Vector2 GetMousePosition()
    {
        return mousePosition;
    }

    private void ChangeActionState(InputAction.CallbackContext ctx)
    {
        if (mousePosition.x < 0 || mousePosition.x > Screen.width || mousePosition.y < 0 || mousePosition.y > Screen.height)
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
        NavMeshHit navMeshHit;
        if (!NavMesh.SamplePosition(RoundPosition(point), out navMeshHit, 0.5f, NavMesh.AllAreas))
            return;
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);
        agent.isStopped = false;
        moveTarget = RoundPosition(point);
        currentCoroutine = StartCoroutine(MoveTask());
    }

    private void Use(GameObject obj)
    {
        IUsableObj usable = obj.GetComponent<IUsableObj>();

        if (usable == null)
            return;
        float distnceToObject = Vector3.Distance(transform.position, obj.transform.position);
        if(distnceToObject > maxDistance)
        {
            currentSelectObj = usable;
            if (currentCoroutine != null)
                StopCoroutine(currentCoroutine);
            agent.isStopped = false;
            if(currentSelectObj is Door)
            {
                Door door = (Door)currentSelectObj;
                moveTarget = door.GetNearSlot();
            }
            else
            {
                moveTarget = obj.transform.position;
            }
            currentCoroutine = StartCoroutine(MoveTask());
            return;
        }
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
        }
        agent.isStopped = true;
        animationPlayer.SetSpeedLocomotion(0f);
        if(currentSelectObj != null)
        {
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
        if(usable is Door)
        {
            Door door = (Door)usable;
            Vector3 slot = door.GetNearSlot();
            agent.Warp(slot);
            transform.rotation = Quaternion.LookRotation(door.GetRootPosition() - transform.position);
            animationPlayer.DoorInteract();
            yield return new WaitForSeconds(animationPlayer.GetDoorInteractTime());
        }
        usable.Use();
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
