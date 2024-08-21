using UnityEngine;
using UnityEngine.InputSystem;

public class EscController : MonoBehaviour
{
    public static EscController instance;
    private bool block = false;
    private MainInputSystem inputActions;
    private void Awake()
    {
        instance = this;
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
        if (block)
            return;

        if(InventoryUI.instance.GetActive())
        {
            InventoryUI.instance.Hide();
            return;
        }

        if(CabinetUI.instance.GetActive())
        {
            CabinetUI.instance.Hide();
            return;
        }

        if(DialogueUI.instance.GetActive())
        {
            DialogueUI.instance.Hide();
            return;
        }

        if(QuestListUI.instance.IsActive())
        {
            QuestListUI.instance.Hide();
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

    public bool IsBlock()
    {
        return block;
    }

    public void SetBlock(bool _block)
    {
        block = _block;
    }
}
