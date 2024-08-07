using UnityEngine;
using UnityEngine.UI;

public class PauseMenuDemo : MonoBehaviour
{
    public static PauseMenuDemo instance;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button quitButton;
    private Canvas canvas;
    private PlayerController player;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
        canvas = GetComponent<Canvas>();
        canvas.enabled = false;
        resumeButton.onClick.AddListener(OnClickResume);
        quitButton.onClick.AddListener(OnClickQuit);
    }

    public void Show()
    {
        canvas.enabled = true;
        Time.timeScale = 0;
        HUDController.instance.SetActiveCanvas(false);
        player.SetInMenu(true);
    }

    private void OnClickResume()
    {
        canvas.enabled = false;
        Time.timeScale = 1;
        HUDController.instance.SetActiveCanvas(true);
        player.SetInMenu(false);
    }

    private void OnClickQuit()
    {
        Debug.Log("Exit");
        Application.Quit();
    }
}
