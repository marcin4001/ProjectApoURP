using System;
using System.Collections;
using Mono.Cecil.Cil;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsPanelNewLevel : MonoBehaviour
{
    public static StatsPanelNewLevel instance;
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject bg_panel;
    [SerializeField] private Button closeBtn;
    [SerializeField] private Button applyBtn;
    [SerializeField] private Image viewPlayerImage;
    [SerializeField] private Sprite[] viewPlayerSprites;
    [SerializeField] private TextMeshProUGUI[] values = new TextMeshProUGUI[4];
    [SerializeField] private Button[] rightButtons;
    [SerializeField] private TextMeshProUGUI pointsValue;
    [SerializeField] private int[] stats = new int[4];
    [SerializeField] private int points;
    private Coroutine coroutine;
    private PlayerController player;
    private bool active = false;

    private void Awake()
    {
        instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
        panel.SetActive(false);
        bg_panel.SetActive(false);
        closeBtn.onClick.AddListener(Close);
        applyBtn.onClick.AddListener(ApplyClick);
        for (int i = 0; i < values.Length; i++)
        {
            int index = i;
            rightButtons[i].onClick.AddListener(() => { RightBtnClick(index); });
        }
    }

    public void Open()
    {
        active = true;
        panel.SetActive(true);
        bg_panel.SetActive(true);
        player.SetInMenu(true);
        CameraMovement.instance.SetBlock(true);
        coroutine = StartCoroutine(ChangePlayerView());

        points = GameParam.instance.skillPoints;
        pointsValue.text = points.ToString();
        stats = PlayerStats.instance.GetStatsAsArray();
        for (int i = 0; i < stats.Length; i++)
        {
            values[i].text = stats[i].ToString();
        }
    }

    private IEnumerator ChangePlayerView()
    {
        int indexImage = 0;
        viewPlayerImage.overrideSprite = viewPlayerSprites[indexImage];
        while (true)
        {
            yield return new WaitForSeconds(1f);
            indexImage++;
            if (indexImage == viewPlayerSprites.Length)
                indexImage = 0;
            viewPlayerImage.overrideSprite = viewPlayerSprites[indexImage];
        }
    }

    public void Close()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }

        panel.SetActive(false);
        bg_panel.SetActive(false);
        active = false;
        CameraMovement.instance.SetBlock(false);
    }

    public void ApplyClick()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }

        panel.SetActive(false);
        bg_panel.SetActive(false);
        active = false;
        CameraMovement.instance.SetBlock(false);
        PlayerStats.instance.SetStats(stats);
        GameParam.instance.skillPoints = points;
        if(points == 0)
        {
            GameParam.instance.isLevelUp = false;
            HUDController.instance.HideNewLevelBtn();
        }
    }

    public void RightBtnClick(int index)
    {
        if (stats[index] >= 10 || points == 0)
            return;
        stats[index] += 1;
        points -= 1;
        pointsValue.text = points.ToString();
        values[index].text = stats[index].ToString();
    }

    public bool GetActive()
    {
        return active;
    }
}
