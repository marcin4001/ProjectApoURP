using UnityEngine;
using UnityEngine.UI;

public class CellGridMapButton : MonoBehaviour
{
    [SerializeField] private string sceneName;
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        MapSceneManager.instance.SetTargetPos();
    }
}
