using UnityEngine;
using UnityEngine.UI;

public class CellGridMapButton : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private string location;
    [SerializeField] private bool notAvailableInDemo = false;
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        if(notAvailableInDemo && GameParam.instance.inDemo)
        {
            ToolTip.instance.SetText("Not available in demo");
            return;
        }
        MapSceneManager.instance.SetNextScene(sceneName);
        MapSceneManager.instance.SetMapSignText(location);
        MapSceneManager.instance.SetTargetPos();
    }

    public void SetNextScene()
    {
        MapSceneManager.instance.SetNextSceneCombat(sceneName);
    }
}
