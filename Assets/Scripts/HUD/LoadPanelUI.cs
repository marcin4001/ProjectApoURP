using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadPanelUI : MonoBehaviour
{
    public static LoadPanelUI instance;
    [SerializeField] private TextMeshProUGUI[] slotTexts;
    [SerializeField] private Button[] buttons;
    [SerializeField] private Button closeBtn;
    [SerializeField] private bool active = false;
    [SerializeField] private GameObject backgroud;
    [SerializeField] private GameObject panel;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        closeBtn.onClick.AddListener(Close);
        backgroud.SetActive(false);
        panel.SetActive(false);
    }

    public void Show()
    {
        active = true;
        backgroud.SetActive(true);
        panel.SetActive(true);
        CreateSaveList();
        for (int i = 0; i < 5; i++)
        {
            int iTemp = i;
            buttons[iTemp].onClick.AddListener(() => LoadClick(iTemp));
        }
    }

    public void LoadClick(int index)
    {
        SaveManager.instance.Load(index);
    }

    public void Close()
    {
        active = false;
        backgroud.SetActive(false);
        panel.SetActive(false);
    }
    public bool GetActive()
    {
        return active;
    }

    public void CreateSaveList()
    {
        for (int i = 0; i < 5; i++)
        {
            slotTexts[i].text = SaveManager.instance.GetSaveInfo(i);
        }
    }
}
