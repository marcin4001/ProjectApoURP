using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsPanelMenu : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject bg_panel;
    [SerializeField] private Button closeBtn;
    [SerializeField] private Button applyBtn;
    [SerializeField] private Image viewPlayerImage;
    [SerializeField] private Sprite[] viewPlayerSprites;
    [SerializeField] private TextMeshProUGUI[] values = new TextMeshProUGUI[4];
    [SerializeField] private Button[] leftButtons;
    [SerializeField] private Button[] rightButtons;
    [SerializeField] private TextMeshProUGUI pointsValue;
    [SerializeField] private int[] stats = new int[4];
    [SerializeField] private int points = 10;
    [SerializeField, TextArea(1, 3)] private string messageText;
    [SerializeField] private Button helpBtn;
    [SerializeField] private BookProfile book;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI strengthText;
    [SerializeField] private TextMeshProUGUI dexterityText;
    [SerializeField] private TextMeshProUGUI technicalText;
    [SerializeField] private TextMeshProUGUI perceptionText;
    [SerializeField] private TextMeshProUGUI pointText;
    [SerializeField] private TextMeshProUGUI applyText;
    private Coroutine coroutine;
    private DemoMenuUI demoMenu;
    private LocalizationCollector loc;

    void Start()
    {
        loc = new LocalizationCollector();
        demoMenu = FindFirstObjectByType<DemoMenuUI>();
        panel.SetActive(false);
        bg_panel.SetActive(false);
        closeBtn.onClick.AddListener(Close);
        applyBtn.onClick.AddListener(ApplyClick);
        helpBtn.onClick.AddListener(HelpClick);
        for (int i = 0; i < values.Length; i++)
        {
            int index = i;
            leftButtons[i].onClick.AddListener(() => {LeftBtnClick(index); });
            rightButtons[i].onClick.AddListener(() => {RightBtnClick(index); });
        }
    }

    public void Open()
    {
        panel.SetActive(true);
        bg_panel.SetActive(true);
        coroutine = StartCoroutine(ChangePlayerView());
        for (int i = 0; i < stats.Length; i++)
        {
            stats[i] = 1;
            values[i].text = stats[i].ToString();
        }
        points = 10;
        pointsValue.text = points.ToString();
        SetLocText();
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
    }

    public void LeftBtnClick(int index)
    {
        Debug.Log(index);
        if(stats[index] == 1)
            return;
        stats[index] -= 1;
        points += 1;
        pointsValue.text = points.ToString();
        values[index].text = stats[index].ToString();
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

    public void ApplyClick()
    {
        if(points >  0)
        {
            MessagePanel.instance.Open(messageText, YesClick);
            return;
        }
        GameParam.instance.SetStatsStart(stats);
        demoMenu.LoadNewGame();
    }

    public void YesClick()
    {
        GameParam.instance.SetStatsStart(stats);
        demoMenu.LoadNewGame();
    }

    public void HelpClick()
    {
        BookReader.instance.Show(book);
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

    public void SetLocText()
    {
        string titleT = "SURVIVAL STATS:";
        string strengthT = "STRENGTH";
        string dexterityT = "DEXTERITY";
        string technicalT = "TECHNICAL";
        string perceptionT = "PERCEPTION";
        string pointsT = "POINTS:";
        string apply = "APPLY";
        titleText.text = loc.LoadTranslate(titleT);
        strengthText.text = loc.LoadTranslate(strengthT);
        dexterityText.text = loc.LoadTranslate(dexterityT);
        technicalText.text = loc.LoadTranslate(technicalT);
        perceptionText.text = loc.LoadTranslate(perceptionT);
        pointText.text = loc.LoadTranslate(pointsT);
        applyText.text = loc.LoadTranslate(apply);
    }
}
