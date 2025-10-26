using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Prologue : MonoBehaviour
{
    [SerializeField] private Animator fadeAnim;
    [SerializeField] private string fadeIOutParam = "FadeIOut";
    [SerializeField, TextArea(3, 6)] private string[] texts;
    [SerializeField] private Sprite[] backgrounds;
    [SerializeField] private TextMeshProUGUI textPrologue;
    [SerializeField] private Image background;
    [SerializeField] private int currentIndex;
    [SerializeField] private string nextScene;
    [SerializeField] private int indexTheme = 1;
    private MainInputSystem inputSystem;
    private bool activeInput = false;
    private LoadingPanel loadingPanel;
    void Start()
    {
        loadingPanel = GetComponent<LoadingPanel>();
        MusicManager.instance.SetMaxVolume(GameParam.instance.maxVolumeTheme);
        MusicManager.instance.SetTheme(indexTheme);
        StartCoroutine(StartPrologue());
    }

    private void Awake()
    {
        inputSystem = new MainInputSystem();
        inputSystem.Player.Enter.performed += Next;
        inputSystem.Player.PlayerAction.performed += Next;
        inputSystem.Enable();
    }

    private void OnEnable()
    {
        inputSystem.Player.Enter.performed += Next;
        inputSystem.Player.PlayerAction.performed += Next;
        inputSystem.Enable();
    }

    private void OnDisable()
    {
        inputSystem.Player.Enter.performed -= Next;
        inputSystem.Player.PlayerAction.performed -= Next;
        inputSystem.Disable();
    }

    private IEnumerator StartPrologue()
    {
        background.overrideSprite = backgrounds[0];
        textPrologue.text = texts[0];
        fadeAnim.SetBool(fadeIOutParam, true);
        yield return new WaitForSeconds(1.5f);
        activeInput = true;
    }

    private IEnumerator NextScreenPrologue()
    {
        fadeAnim.SetBool(fadeIOutParam, false);
        yield return new WaitForSeconds(1.5f);
        background.overrideSprite = backgrounds[currentIndex];
        textPrologue.text = texts[currentIndex];
        fadeAnim.SetBool(fadeIOutParam, true);
        yield return new WaitForSeconds(1.5f);
        activeInput = true;
    }

    private IEnumerator LoadScene()
    {
        fadeAnim.SetBool(fadeIOutParam, false);
        yield return new WaitForSeconds(1.5f);
        loadingPanel.Show();
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(nextScene);
    }

    private void Next(InputAction.CallbackContext ctx)
    {
        if(!activeInput)
            return;
        currentIndex += 1;
        activeInput = false;
        if(currentIndex < backgrounds.Length)
        {
            StartCoroutine(NextScreenPrologue());
        }
        else
        {
            StartCoroutine(LoadScene());
        }
    }
}
