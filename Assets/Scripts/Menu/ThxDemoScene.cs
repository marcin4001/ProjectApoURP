using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ThxDemoScene : MonoBehaviour
{
    [SerializeField] private Animator fadeAnim;
    [SerializeField] private string fadeOutParam = "FadeIOut";
    [SerializeField] private bool activeInput = false;
    private MainInputSystem inputSystem;

    private void Awake()
    {
        inputSystem = new MainInputSystem();
        inputSystem.Player.AnyKey.performed += ExitInput;
        inputSystem.Enable();
    }

    void OnEnable()
    {
        inputSystem.Player.AnyKey.performed += ExitInput;
        inputSystem.Enable();
    }

    void OnDisable()
    {
        inputSystem.Player.AnyKey.performed -= ExitInput;
        inputSystem.Disable();
    }
    void Start()
    {
        StartCoroutine(Enter());
    }

    private IEnumerator Enter()
    {
        fadeAnim.SetBool(fadeOutParam, true);
        yield return new WaitForSeconds(1.5f);
        activeInput = true;
    }

    private IEnumerator Exit()
    {
        fadeAnim.SetBool(fadeOutParam, false);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(0);
    }

    private void ExitInput(InputAction.CallbackContext ctx)
    {
        if(!activeInput)
            return;
        activeInput = false;
        StartCoroutine(Exit());
    }
}
