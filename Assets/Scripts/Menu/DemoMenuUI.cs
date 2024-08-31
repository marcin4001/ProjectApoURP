using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DemoMenuUI : MonoBehaviour
{
    [SerializeField] private string sceneStartName = "MainScene";
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;
    void Start()
    {
        playButton.onClick.AddListener(OnClickPlay);
        quitButton.onClick.AddListener(OnClickQuit);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void OnClickPlay()
    {
        SceneManager.LoadScene(sceneStartName);
    }

    private void OnClickQuit()
    {
        Debug.Log("Exit");
        Application.Quit();
    }
}
