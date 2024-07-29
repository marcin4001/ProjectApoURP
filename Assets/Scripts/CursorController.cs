using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;

public class CursorController : MonoBehaviour
{
    [SerializeField] private Texture2D reachableCursor;
    [SerializeField] private Texture2D unreachableCursor;
    [SerializeField] private Texture2D useCursor;
    [SerializeField] private Texture2D lookCursor;
    [SerializeField] private LayerMask layer;
    [SerializeField] private Vector2 hotspot;
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
        mainInputSystem.Enable();
    }

    void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
        cam = FindFirstObjectByType<Camera>();
    }

    private void OnEnable()
    {
        mainInputSystem.Player.ChangeStateAction.performed += UpdateCursor;
        mainInputSystem.Player.MousePos.performed += UpdateCursor;
        mainInputSystem.Player.CameraMove.performed += UpdateCursor;
        mainInputSystem.Enable();
    }

    private void OnDisable()
    {
        mainInputSystem.Player.ChangeStateAction.performed -= UpdateCursor;
        mainInputSystem.Player.MousePos.performed -= UpdateCursor;
        mainInputSystem.Player.CameraMove.performed -= UpdateCursor;
        mainInputSystem.Disable();
    }

    public void UpdateCursor(InputAction.CallbackContext ctx)
    {
        StartCoroutine(UpdateCursor());
    }

    private IEnumerator UpdateCursor()
    {
        yield return new WaitForEndOfFrame();
        mousePosition = player.GetMousePosition();

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

    public void SetMoveCursor()
    {
        Ray ray = cam.ScreenPointToRay(mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000f, layer))
        {
            Vector3 position = player.RoundPosition(hit.point);
            NavMeshHit navMeshHit;
            if (NavMesh.SamplePosition(position, out navMeshHit, 0.5f, NavMesh.AllAreas))
                Cursor.SetCursor(reachableCursor, hotspot, CursorMode.Auto);
            else
                Cursor.SetCursor(unreachableCursor, hotspot, CursorMode.Auto);
                
        }
    }

    public void SetUseCursor()
    {
        Cursor.SetCursor(useCursor, hotspot, CursorMode.Auto);
    }

    public void SetLookCursor()
    {
        Cursor.SetCursor(lookCursor, hotspot, CursorMode.Auto);
    }
}
