using UnityEngine;
using UnityEngine.UI;

public class CellGridMapButton : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private string location;
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        MapSceneManager.instance.SetNextScene(sceneName);
        MapSceneManager.instance.SetMapSignText(location);
        MapSceneManager.instance.SetTargetPos();
    }
}
