using UnityEngine;
using UnityEngine.UI;

public class MapSceneManager : MonoBehaviour
{
    public static MapSceneManager instance;
    [SerializeField] private GameObject grid;
    [SerializeField] private RectTransform map;
    [SerializeField] private Button gridButton;
    [SerializeField] private Button enterButton;
    [SerializeField] private Sprite enterBtnActiveSprite;
    [SerializeField] private Sprite enterBtnInactiveSprite;
    [SerializeField] private RectTransform target;
    private Camera cam;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        cam = FindFirstObjectByType<Camera>();
        grid.SetActive(false);
        gridButton.onClick.AddListener(ShowGrid);
        enterButton.onClick.AddListener(OnClickEnter);
    }

    private void ShowGrid()
    {
        if(grid.activeSelf)
        {
            grid.SetActive(false);
        }
        else
        {
            grid.SetActive(true);
        }
    }

    private void OnClickEnter()
    {

    }


    public void SetTargetPos()
    {
        Vector2 newPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(map, Input.mousePosition, cam, out newPoint);
        target.anchoredPosition = newPoint;
    }
}
