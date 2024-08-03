using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class CursorController : MonoBehaviour
{
    [SerializeField] private Texture2D defaultCursor;
    [SerializeField] private Texture2D reachableCursor;
    [SerializeField] private Texture2D unreachableCursor;
    [SerializeField] private Texture2D useCursor;
    [SerializeField] private Texture2D lookCursor;
    [SerializeField] private LayerMask layer;
    [SerializeField] private RectTransform canvas;
    [SerializeField] private RawImage cursorImage;
    [SerializeField] private float scaleFactorSmallRes = 0.75f;
    private RectTransform cursorRect;
    private PlayerController player;
    private Camera cam;
    private MainInputSystem mainInputSystem;
    private Vector2 mousePosition;

    private void Awake()
    {
        mainInputSystem = new MainInputSystem();
        mainInputSystem.Player.MousePos.performed += UpdateCursor;
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
    }

    private void OnEnable()
    {
        mainInputSystem.Player.ChangeStateAction.performed += UpdateCursor;
        mainInputSystem.Player.MousePos.performed += UpdateCursor;
        mainInputSystem.Player.CameraMove.performed += UpdateCursor;
        mainInputSystem.Player.PlayerAction.performed += UpdateCursor;
        mainInputSystem.Enable();
    }

    private void OnDisable()
    {
        mainInputSystem.Player.ChangeStateAction.performed -= UpdateCursor;
        mainInputSystem.Player.MousePos.performed -= UpdateCursor;
        mainInputSystem.Player.CameraMove.performed -= UpdateCursor;
        mainInputSystem.Player.PlayerAction.performed -= UpdateCursor;
        mainInputSystem.Disable();
    }

    public void UpdateCursor(InputAction.CallbackContext ctx)
    {
        mousePosition = player.GetMousePosition();
        MoveCursor();
        SetScaleCursor();
        StartCoroutine(UpdateCursor());
    }

    private IEnumerator UpdateCursor()
    {
        yield return new WaitForEndOfFrame();

        if (HUDController.instance.PointerOnHUD() || player.IsUsingItem() || player.InMenu())
        {
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
