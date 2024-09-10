using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

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
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private float segmentLen = 10f;
    [SerializeField] private float gapSegment = 5f;
    [SerializeField] private float moveSpeed = 35f;
    [SerializeField] private string nextScene;
    private Image buttonEnterImage;
    private List<RectTransform> pathList = new List<RectTransform>();
    private Camera cam;
    private Coroutine currentCoroutine;
    private float gameTime = 0f;
    private bool blockEnterButton = false;
    private bool blockSetTarget = false;

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
        buttonEnterImage = enterButton.GetComponent<Image>();
        if(GameParam.instance != null)
        {
            gameTime = GameParam.instance.currentTime;
            playerSign.anchoredPosition = GameParam.instance.mapPosition;
            nextScene = GameParam.instance.prevScene;
        }
        timeText.text = GetCurrentTimeString();
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
        if(!blockEnterButton)
        {
            GameParam.instance.currentTime = gameTime;
            GameParam.instance.mapPosition = playerSign.anchoredPosition;
            SceneManager.LoadScene(nextScene);
        }
    }


    public void SetTargetPos()
    {
        if (blockSetTarget)
            return;
        Vector2 newPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(map, Input.mousePosition, cam, out newPoint);
        target.gameObject.SetActive(true);
        target.anchoredPosition = newPoint;
        DrawPath();
        if(currentCoroutine != null)
            StopCoroutine(currentCoroutine);
        currentCoroutine = StartCoroutine(MoveToTarget());
    }

    public void SetNextScene(string _scene)
    {
        if (blockSetTarget)
            return;
        nextScene = _scene;
    }

    private IEnumerator MoveToTarget()
    {
        blockSetTarget = true;
        Vector2 playerPos = playerSign.anchoredPosition;
        Vector2 targetPos = target.anchoredPosition;
        float distance = Vector2.Distance(playerPos, targetPos);
        float distanceTraveled = 0;
        buttonEnterImage.overrideSprite = enterBtnInactiveSprite;
        blockEnterButton = true;
        CursorController.instance.SetIsWait(true);
        while (distance > 0.5f)
        {
            playerPos = playerSign.anchoredPosition;
            playerSign.anchoredPosition = Vector2.MoveTowards(playerPos, targetPos, Time.deltaTime * moveSpeed);
            distanceTraveled += Time.deltaTime * moveSpeed;
            if(distanceTraveled >= 18f)
            {
                distanceTraveled = 0f;
                gameTime += 1f;
                if(gameTime >= 24f)
                    gameTime = 0f;
                timeText.text = GetCurrentTimeString();
            }
            DrawPath();
            yield return new WaitForEndOfFrame();
            distance = Vector2.Distance(playerPos, targetPos);
        }
        playerSign.anchoredPosition = targetPos;
        target.gameObject.SetActive(false);
        blockEnterButton = false;
        blockSetTarget = false;
        CursorController.instance.SetIsWait(false);
        buttonEnterImage.overrideSprite = enterBtnActiveSprite;
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

    private string GetCurrentTimeString()
    {
        int hour = Mathf.FloorToInt(gameTime);
        float fractionalHour = gameTime - hour;
        int minutes = Mathf.FloorToInt(fractionalHour * 60);

        string period = "AM";
        if (hour >= 12)
        {
            period = "PM";
            if (hour >= 13)
                hour -= 12;
        }
        if (hour == 0)
            hour = 12;

        return $"{hour.ToString("D2")}:{minutes.ToString("D2")} {period}";
    }
}
