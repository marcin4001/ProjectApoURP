using System.CodeDom.Compiler;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CursorController : MonoBehaviour
{
    public static CursorController instance;
    [SerializeField] private Texture2D defaultCursor;
    [SerializeField] private Texture2D reachableCursor;
    [SerializeField] private Texture2D unreachableCursor;
    [SerializeField] private Texture2D useCursor;
    [SerializeField] private Texture2D lookCursor;
    [SerializeField] private Texture2D upCursor;
    [SerializeField] private Texture2D downCursor;
    [SerializeField] private Texture2D[] waitCursor;
    [SerializeField] private bool isWait = false;
    [SerializeField] private int waitCursorIndex = 0;
    [SerializeField] private LayerMask layer;
    [SerializeField] private RectTransform canvas;
    [SerializeField] private RawImage cursorImage;
    [SerializeField] private float scaleFactorSmallRes = 0.75f;
    [SerializeField] private bool inMenu = false;
    private RectTransform cursorRect;
    private PlayerController player;
    private Camera cam;
    private MainInputSystem mainInputSystem;
    [SerializeField] private Vector2 mousePosition;
    private Coroutine waitCoroutine;

    private void Awake()
    {
        instance = this;
        mainInputSystem = new MainInputSystem();
        mainInputSystem.Player.MousePos.performed += UpdateMousePos;
        mainInputSystem.Player.ChangeStateAction.performed += UpdateCursor;
        mainInputSystem.Player.CameraMove.performed += UpdateCursor;
        mainInputSystem.Player.PlayerAction.performed += UpdateCursor;
        mainInputSystem.Enable();
    }

    void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
        cam = FindFirstObjectByType<Camera>();
        cursorRect = cursorImage.GetComponent<RectTransform>();
        Cursor.visible = false;
        if(inMenu)
            cursorImage.texture = defaultCursor;
    }

    private void OnEnable()
    {
        mainInputSystem.Player.ChangeStateAction.performed += UpdateCursor;
        mainInputSystem.Player.MousePos.performed += UpdateMousePos;
        mainInputSystem.Player.CameraMove.performed += UpdateCursor;
        mainInputSystem.Player.PlayerAction.performed += UpdateCursor;
        mainInputSystem.Enable();
    }

    private void OnDisable()
    {
        mainInputSystem.Player.ChangeStateAction.performed -= UpdateCursor;
        mainInputSystem.Player.MousePos.performed -= UpdateMousePos;
        mainInputSystem.Player.CameraMove.performed -= UpdateCursor;
        mainInputSystem.Player.PlayerAction.performed -= UpdateCursor;
        mainInputSystem.Disable();
    }

    public void UpdateCursor(InputAction.CallbackContext ctx)
    {
        MoveCursor();
        SetScaleCursor();
        if(!isWait && inMenu)
        {
            cursorImage.texture = defaultCursor;
            return;
        }
        if(!isWait)
            StartCoroutine(UpdateCursor());
    }

    public void UpdateMousePos(InputAction.CallbackContext ctx)
    {
        mousePosition = ctx.ReadValue<Vector2>();
        UpdateCursor(ctx);
    }

    private IEnumerator UpdateCursor()
    {
        yield return new WaitForEndOfFrame();

        if (HUDController.instance.PointerOnHUD() || player.IsUsingItem() || player.InMenu())
        {
            if (HUDController.instance.PointerOnUpButtonConsole())
                cursorImage.texture = upCursor;
            else if(HUDController.instance.PointerOnDownButtonConsole())
                cursorImage.texture = downCursor;
            else
                cursorImage.texture = defaultCursor;
        }
        else
        {
            PlayerActionState state = player.GetState();
            switch (state)
            {
                case PlayerActionState.Move:
                    SetMoveCursor();
                    break;
                case PlayerActionState.Use:
                    SetUseCursor();
                    break;
                case PlayerActionState.Look:
                    SetLookCursor();
                    break;
            }
        }
    }

    public void SetIsWait(bool _isWait)
    {
        isWait = _isWait;
        if(isWait)
        {
            if(waitCoroutine == null)
                waitCoroutine = StartCoroutine(AnimateWaitCursor());
        }
        else
        {
            if (waitCoroutine != null)
            {
                StopCoroutine(waitCoroutine);
                waitCoroutine = null;
                if(inMenu)
                {
                    cursorImage.texture = defaultCursor;
                    return;
                }
                StartCoroutine(UpdateCursor());
            }
        }
    }

    private IEnumerator AnimateWaitCursor()
    {
        waitCursorIndex = 0;
        while (true)
        {
            cursorImage.texture = waitCursor[waitCursorIndex];
            yield return new WaitForSeconds(0.1f);
            waitCursorIndex++;
            if(waitCursorIndex >= waitCursor.Length)
                waitCursorIndex = 0;
        }
    }

    private void MoveCursor()
    {
        cursorRect.position = mousePosition;
    }

    private void SetScaleCursor()
    {
        if(Screen.width > 1280)
            cursorRect.localScale = Vector3.one;
        else
            cursorRect.localScale = Vector3.one * scaleFactorSmallRes;
    }

    public void SetMoveCursor()
    {
        Ray ray = cam.ScreenPointToRay(mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000f, layer))
        {
            Vector3 position = hit.point;
            NavMeshHit navMeshHit;
            if (NavMesh.SamplePosition(position, out navMeshHit, 0.5f, NavMesh.AllAreas))
                cursorImage.texture = reachableCursor;
            else
                cursorImage.texture = unreachableCursor;
                
        }
    }

    public void SetUseCursor()
    {
        cursorImage.texture = useCursor;
    }

    public void SetLookCursor()
    {
        cursorImage.texture = lookCursor;
    }
}
