using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestListUI : MonoBehaviour
{
    public static QuestListUI instance;
    [SerializeField] private Button closeButton;
    [SerializeField] private TextMeshProUGUI statsText;
    [SerializeField] private Transform playerStats;
    [SerializeField] private GameObject cameraPlayerStats;
    [SerializeField] private bool active;
    private Canvas canvas;
    private PlayerController player;
    private Coroutine coroutine;
    private void Awake()
    {
        instance = this;
        canvas = GetComponent<Canvas>();
        player = FindFirstObjectByType<PlayerController>();
        cameraPlayerStats.SetActive(false);
        canvas.enabled = false;
        closeButton.onClick.AddListener(Hide);
    }

    public void Show()
    {
        active = true;
        canvas.enabled = true;
        player.SetInMenu(true);
        CameraMovement.instance.SetBlock(true);
        statsText.text = PlayerStats.instance.GetStatsText();
        cameraPlayerStats.SetActive(true);
        coroutine = StartCoroutine(RotatePlayer());
    }

    public void Hide()
    {
        active = false;
        canvas.enabled = false;
        player.SetInMenu(false);
        CameraMovement.instance.SetBlock(false);
        cameraPlayerStats.SetActive(false);
        if(coroutine != null)
            StopCoroutine(coroutine);
    }

    public bool IsActive()
    {
        return active;
    }

    private IEnumerator RotatePlayer()
    {
        playerStats.rotation = Quaternion.Euler(Vector3.zero);
        while (true)
        {
            yield return new WaitForSeconds(1f);
            playerStats.Rotate(0f, -90f, 0f);
        }
    }
}
