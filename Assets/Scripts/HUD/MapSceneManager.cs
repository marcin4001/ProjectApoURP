using System.Collections.Generic;
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
    [SerializeField] private RectTransform playerSign;
    [SerializeField] private RectTransform target;
    [SerializeField] private Transform pathParent;
    [SerializeField] private GameObject pathSegment;
    [SerializeField] private float segmentLen = 10f;
    [SerializeField] private float gapSegment = 5f;
    private List<RectTransform> pathList = new List<RectTransform>();
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
        target.gameObject.SetActive(false);
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
        target.gameObject.SetActive(true);
        target.anchoredPosition = newPoint;
        DrawPath();
    }

    private void DrawPath()
    {
        foreach(RectTransform path in pathList)
        {
            Destroy(path.gameObject);
        }
        pathList.Clear();
        Vector2 playerPos = playerSign.anchoredPosition;
        Vector2 targetPos = target.anchoredPosition;
        float distance = Vector2.Distance(playerPos, targetPos);
        Vector2 directionPath = (playerPos - targetPos).normalized;
        int countSegment = Mathf.RoundToInt(distance / (segmentLen + gapSegment));
        for(int i = 0; i < countSegment; i++)
        {
            RectTransform newSegment = Instantiate(pathSegment, pathParent).GetComponent<RectTransform>();
            newSegment.name = $"Segment {i + 1}";
            pathList.Add(newSegment);
            newSegment.pivot = new Vector2(0f, 0.5f);
            newSegment.anchoredPosition = targetPos + directionPath * (i * (segmentLen + gapSegment));
            newSegment.sizeDelta = new Vector2(segmentLen, 2f);
            float angleSegment = Mathf.Atan2(directionPath.y, directionPath.x) * Mathf.Rad2Deg;
            newSegment.rotation = Quaternion.Euler(0f, 0f, angleSegment);
        }
    }
}
