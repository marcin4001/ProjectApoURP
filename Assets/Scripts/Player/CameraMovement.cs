using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CameraMovement : MonoBehaviour
{
    public static CameraMovement instance;
    [SerializeField] private float borderThickness = 10f;
    private float speed = 6f;
    [SerializeField] private Transform limitPointA;
    [SerializeField] private Transform limitPointB;
    [SerializeField] private Vector3 minLimitVector = Vector3.zero;
    [SerializeField] private Vector3 maxLimitVector = Vector3.zero;
    [SerializeField] private Camera m_Camera;
    [SerializeField] private float maxZoom = 5f;
    [SerializeField] private float minZoom = 3f;
    [SerializeField] private float speedZoom = 20f;
    [SerializeField] private bool block = false;
    private MainInputSystem inputSystem;
    private PlayerController playerController;
    [SerializeField] private Vector2 mousePosition;
    private Vector2 inputMove;

    void Awake()
    {
        instance = this;
        inputSystem = new MainInputSystem();
        inputSystem.Player.MousePos.performed += SetMousePosition;
        inputSystem.Player.CameraMove.performed += SetInputMove;
        inputSystem.Player.CameraMove.canceled += SetInputMove;
        inputSystem.Player.CenterPlayer.performed += CenterCameraToPlayer;
        inputSystem.Player.Zoom.performed += Zoom;
        inputSystem.Enable();
    }

    private void OnEnable()
    {
        inputSystem.Player.MousePos.performed += SetMousePosition;
        inputSystem.Player.CameraMove.performed += SetInputMove;
        inputSystem.Player.CameraMove.canceled += SetInputMove;
        inputSystem.Player.CenterPlayer.performed += CenterCameraToPlayer;
        inputSystem.Player.Zoom.performed += Zoom;
        inputSystem.Enable();
    }

    private void OnDisable()
    {
        inputSystem.Player.MousePos.performed -= SetMousePosition;
        inputSystem.Player.CameraMove.performed -= SetInputMove;
        inputSystem.Player.CameraMove.canceled -= SetInputMove;
        inputSystem.Player.CenterPlayer.performed -= CenterCameraToPlayer;
        inputSystem.Player.Zoom.performed -= Zoom;
        inputSystem.Disable();
    }

    void Start()
    {
        playerController = FindFirstObjectByType<PlayerController>();
        m_Camera.orthographicSize = maxZoom;
        if(limitPointA != null && limitPointB != null)
        {
            minLimitVector.x = Mathf.Min(limitPointA.position.x, limitPointB.position.x);
            minLimitVector.z = Mathf.Min(limitPointA.position.z, limitPointB.position.z);
            maxLimitVector.x = Mathf.Max(limitPointA.position.x, limitPointB.position.x);
            maxLimitVector.z = Mathf.Max(limitPointA.position.z, limitPointB.position.z);
        }
        transform.eulerAngles = GameParam.instance.cameraPivotRot;
        m_Camera.nearClipPlane = -5f;
        m_Camera.transform.localPosition = GameParam.instance.posCamera;
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
        CenterCameraToPlayer();
    }

    public void CenterCameraToPlayer()
    {
        Vector3 cameraPosition = transform.position;
        cameraPosition = playerController.transform.position;
        cameraPosition.y = 0f;
        transform.position = cameraPosition;
    }

    public void CenterCameraTo(Transform obj)
    {
        Vector3 cameraPosition = transform.position;
        cameraPosition = obj.position;
        cameraPosition.y = 0f;
        transform.position = cameraPosition;
    }

    private void Zoom(InputAction.CallbackContext ctx)
    {
        if (block)
            return;
        float value = ctx.ReadValue<float>();
        float currentZoom = m_Camera.orthographicSize + value * -Time.deltaTime * speedZoom;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
        m_Camera.orthographicSize = currentZoom;
    }

    private void Update()     
    {
        if (block)
            return;
        if (mousePosition.x < 0 || mousePosition.x > Screen.width || mousePosition.y < 0 || mousePosition.y > Screen.height)
            return;
        if(inputMove == Vector2.zero)
            MoveCameraBorder();
        MoveCameraInput();
        ClampPosCamera();
    }

    private void MoveCameraBorder()
    {
        if(IsMouseOverButton())
            return;
        Vector3 forwardCam = transform.forward;
        Vector3 rightCam = transform.right;

        forwardCam.y = 0;
        rightCam.y = 0;

        forwardCam.Normalize();
        rightCam.Normalize();

        Vector3 dir = new Vector3(0f, 0f, 0f);
        if (mousePosition.y >= Screen.height - borderThickness)
            dir += forwardCam;
        if(mousePosition.y < borderThickness)
            dir -= forwardCam;
        if (mousePosition.x >= Screen.width - borderThickness)
            dir += rightCam;
        if(mousePosition.x < borderThickness)
            dir -= rightCam;

        transform.position += dir * speed * Time.deltaTime;
    }

    private void MoveCameraInput()
    {
        Vector3 forwardCam = transform.forward;
        Vector3 rightCam = transform.right;

        forwardCam.y = 0;
        rightCam.y = 0;

        forwardCam.Normalize();
        rightCam.Normalize();

        Vector3 dir = forwardCam * inputMove.y + rightCam * inputMove.x;
        transform.position += dir * speed * Time.deltaTime;
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

    public bool IsBlock()
    {
        return block;
    }

    public void SetBlock(bool value)
    {
        block = value;
    }

    public Transform GetTransformCamera()
    {
        return m_Camera.transform;
    }

    public bool ObjectInFov(Transform obj)
    {
        Vector3 pointInFov = m_Camera.WorldToViewportPoint(obj.position);
        return (pointInFov.z > 0 
                && pointInFov.x >= 0 && pointInFov.x <= 1
                && pointInFov.y >= 0 && pointInFov.y <= 1);
    }

    private bool IsMouseOverButton()
    {
        PointerEventData data = new PointerEventData(EventSystem.current);
        data.position = mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(data, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.GetComponent<Button>() != null)
            {
                //Debug.Log(result.gameObject.name);
                return true;
            }
        }

        return false;
    }
}
