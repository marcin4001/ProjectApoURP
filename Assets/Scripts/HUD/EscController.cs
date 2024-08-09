using UnityEngine;
using UnityEngine.InputSystem;

public class EscController : MonoBehaviour
{
    private MainInputSystem inputActions;
    private void Awake()
    {
        inputActions = new MainInputSystem();
        inputActions.Player.Pause.performed += OnEscClick;
        inputActions.Enable();
    }

    private void OnEnable()
    {
        inputActions.Player.Pause.performed += OnEscClick;
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Player.Pause.performed -= OnEscClick;
        inputActions.Disable();
    }

    public void OnEscClick(InputAction.CallbackContext ctx)
    {
        if(InventoryUI.instance.GetActive())
        {
            InventoryUI.instance.Hide();
            return;
        }

        if(CabinetUI.instance.GetActive())
        {
            Debug.Log("Weszlo");
            CabinetUI.instance.Hide();
            return;
        }

        if (PauseMenuDemo.instance.GetActive()) 
        {
            PauseMenuDemo.instance.Hide();
        }
        else
        {
            PauseMenuDemo.instance.Show();
        }
    }
}
