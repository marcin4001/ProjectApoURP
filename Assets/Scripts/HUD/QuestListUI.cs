using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestListUI : MonoBehaviour
{
    public static QuestListUI instance;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button currentButton;
    [SerializeField] private Button completeButton;
    [SerializeField] private TextMeshProUGUI statsText;
    [SerializeField] private TextMeshProUGUI questListText;
    [SerializeField] private Transform playerStats;
    [SerializeField] private GameObject cameraPlayerStats;
    [SerializeField] private Image viewPlayerImage;
    [SerializeField] private Sprite[] viewPlayerSprites;
    [SerializeField] private RawImage oldImage;
    [SerializeField] private bool active;
    [SerializeField] private string separator = "########################################";
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
        currentButton.onClick.AddListener(CreateCurrentQuestList);
        completeButton.onClick.AddListener(CreateCompleteQuestList);
        if (oldImage != null)
            oldImage.gameObject.SetActive(false);
    }

    public void Show()
    {
        active = true;
        canvas.enabled = true;
        player.SetInMenu(true);
        CameraMovement.instance.SetBlock(true);
        statsText.text = PlayerStats.instance.GetStatsText();
        //cameraPlayerStats.SetActive(true);
        coroutine = StartCoroutine(ChangePlayerView());
        CreateCurrentQuestList();
    }

    public void Hide()
    {
        active = false;
        canvas.enabled = false;
        player.SetInMenu(false);
        CameraMovement.instance.SetBlock(false);
        //cameraPlayerStats.SetActive(false);
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

    private IEnumerator ChangePlayerView()
    {
        int indexImage = 0;
        viewPlayerImage.overrideSprite = viewPlayerSprites[indexImage];
        while (true)
        {
            yield return new WaitForSeconds(1f);
            indexImage++;
            if(indexImage == viewPlayerSprites.Length)
                indexImage = 0;
            viewPlayerImage.overrideSprite = viewPlayerSprites[indexImage];
        }
    }

    private void CreateCurrentQuestList()
    {
        string questList = $"{separator}\nCurrent Quests\n{separator}\n";
        List<Quest> quests = QuestController.instance.GetQuests();
        List<Quest> currentQuest = quests.FindAll(x => !x.complete && !x.hidden);
        if(currentQuest.Count == 0)
        {
            questList += "You don't have current Quests!";
            questListText.text = questList;
            return;
        }
        foreach (Quest quest in currentQuest)
        {
            questList += $"{quest.questTitle}\nOwner: {quest.owner}\nLocation: {quest.location}\n{separator}\n";
        }
        questListText.text = questList;
    }

    private void CreateCompleteQuestList()
    {
        string questList = $"{separator}\nComplete Quests\n{separator}\n";
        List<Quest> quests = QuestController.instance.GetQuests();
        List<Quest> completeQuest = quests.FindAll(x => x.complete && !x.hidden);
        if (completeQuest.Count == 0)
        {
            questList += "You don't have current Quests!";
            questListText.text = questList;
            return;
        }
        foreach (Quest quest in completeQuest)
        {
            questList += $"{quest.questTitle}\nOwner: {quest.owner}\nLocation: {quest.location}\n{separator}\n";
        }
        questListText.text = questList;
    }
}
