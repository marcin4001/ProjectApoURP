using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.Rendering.DebugUI;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float borderThickness = 10f;
    [SerializeField] private float speedMovingCamera = 10f;
    [SerializeField] private Transform limitPointA;
    [SerializeField] private Transform limitPointB;
    [SerializeField] private Vector3 minLimitVector = Vector3.zero;
    [SerializeField] private Vector3 maxLimitVector = Vector3.zero;
    private MainInputSystem inputSystem;
    private PlayerController playerController;
    private Vector2 mousePosition;
    private Vector2 inputMove;

    void Awake()
    {
        inputSystem = new MainInputSystem();
        inputSystem.Player.MousePos.performed += SetMousePosition;
        inputSystem.Player.CameraMove.performed += SetInputMove;
        inputSystem.Player.CameraMove.canceled += SetInputMove;
        inputSystem.Player.CenterPlayer.performed += CenterCameraToPlayer;
        inputSystem.Enable();
    }

    private void OnEnable()
    {
        inputSystem.Player.MousePos.performed += SetMousePosition;
        inputSystem.Player.CameraMove.performed += SetInputMove;
        inputSystem.Player.CameraMove.canceled += SetInputMove;
        inputSystem.Player.CenterPlayer.performed += CenterCameraToPlayer;
        inputSystem.Enable();
    }

    private void OnDisable()
    {
        inputSystem.Player.MousePos.performed -= SetMousePosition;
        inputSystem.Player.CameraMove.performed -= SetInputMove;
        inputSystem.Player.CameraMove.canceled -= SetInputMove;
        inputSystem.Player.CenterPlayer.performed -= CenterCameraToPlayer;
        inputSystem.Disable();
    }

    void Start()
    {
        playerController = FindFirstObjectByType<PlayerController>();

        if(limitPointA != null && limitPointB != null)
        {
            minLimitVector.x = Mathf.Min(limitPointA.position.x, limitPointB.position.x);
            minLimitVector.z = Mathf.Min(limitPointA.position.z, limitPointB.position.z);
            maxLimitVector.x = Mathf.Max(limitPointA.position.x, limitPointB.position.x);
            maxLimitVector.z = Mathf.Max(limitPointA.position.z, limitPointB.position.z);
        }
    }

    private void SetMousePosition(InputAction.CallbackContext ctx)
    {
        mousePosition = ctx.ReadValue<Vector2>();
    }

    private void SetInputMove(InputAction.CallbackContext ctx)
    {
        inputMove = ctx.ReadValue<Vector2>();
    }

    private void CenterCameraToPlayer(InputAction.CallbackContext ctx)
    {
        Vector3 cameraPosition = transform.position;
        cameraPosition = playerController.transform.position;
        cameraPosition.y = 0f;
        transform.position = cameraPosition;
    }

    private void Update()     
    {
        if (mousePosition.x < 0 || mousePosition.x > Screen.width || mousePosition.y < 0 || mousePosition.y > Screen.height)
            return;
        if(inputMove == Vector2.zero)
            MoveCameraBorder();
        MoveCameraInput();
        ClampPosCamera();
    }

    private void MoveCameraBorder()
    {
        Vector3 cameraPos = transform.position;

        if (mousePosition.y >= Screen.height - borderThickness)
            cameraPos.z += speedMovingCamera * Time.deltaTime;
        if (mousePosition.y < borderThickness)
            cameraPos.z -= speedMovingCamera * Time.deltaTime;
        if (mousePosition.x >= Screen.width - borderThickness)
            cameraPos.x += speedMovingCamera * Time.deltaTime;
        if (mousePosition.x < borderThickness)
            cameraPos.x -= speedMovingCamera * Time.deltaTime;

        transform.position = cameraPos;
    }

    private void MoveCameraInput()
    {
        Vector3 cameraPos = transform.position;

        cameraPos.x += inputMove.x * speedMovingCamera * Time.deltaTime;
        cameraPos.z += inputMove.y * speedMovingCamera * Time.deltaTime;

        transform.position = cameraPos;
    }

    private void ClampPosCamera()
    {
        if (limitPointA != null && limitPointB != null)
        {
            Vector3 camPosition = transform.position;
            camPosition.x = Mathf.Clamp(camPosition.x, minLimitVector.x, maxLimitVector.x);
            camPosition.z = Mathf.Clamp(camPosition.z, minLimitVector.z, maxLimitVector.z);
            transform.position = camPosition;
        }
    }
}
