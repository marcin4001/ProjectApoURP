using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ThxDemoScene : MonoBehaviour
{
    [SerializeField] private Animator fadeAnim;
    [SerializeField] private string fadeOutParam = "FadeIOut";
    [SerializeField] private bool activeInput = false;
    [SerializeField] private Button steamBtn;
    [SerializeField] private string steamURL = "https://store.steampowered.com/app/4546490/Arkansas_2125/";
    private MainInputSystem inputSystem;

    private void Awake()
    {
        inputSystem = new MainInputSystem();
        inputSystem.Player.Enter.performed += ExitInput;
        inputSystem.Enable();
    }

    void OnEnable()
    {
        inputSystem.Player.Enter.performed += ExitInput;
        inputSystem.Enable();
    }

    void OnDisable()
    {
        inputSystem.Player.Enter.performed -= ExitInput;
        inputSystem.Disable();
    }
    void Start()
    {
        StartCoroutine(Enter());
        steamBtn?.onClick.AddListener(OnClickSteam);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
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

    public void OnClickSteam()
    {
        Application.OpenURL(steamURL);
    }
}
