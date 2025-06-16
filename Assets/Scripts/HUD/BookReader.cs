using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BookReader : MonoBehaviour
{
    public static BookReader instance;
    [SerializeField] private Button closeBtn;
    [SerializeField] private Button leftBtn;
    [SerializeField] private Button rightBtn;
    [SerializeField] private GameObject panel;
    [SerializeField] private bool active = false;
    [SerializeField] private TextMeshProUGUI screenText;
    [SerializeField] private TextMeshProUGUI pageText;
    [SerializeField] private BookProfile book;
    private PlayerController player;
    private int pageIndex = 0;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        panel.SetActive(false);
        closeBtn.onClick.AddListener(Hide);
        rightBtn.onClick.AddListener(RightBtn);
        leftBtn.onClick.AddListener(LeftBtn);
        player = FindFirstObjectByType<PlayerController>();
    }

    
    public void Show(BookProfile profile)
    {
        active = true;
        panel.SetActive(true);
        player.SetInMenu(true);
        CameraMovement.instance.SetBlock(true);
        book = profile;
        if(book != null)
        {
            if(book.pages.Count ==  0)
                return;
            screenText.text = book.pages[0].text;
            pageText.text = $"1/{book.pages.Count}";
            pageIndex = 0;
        }
    }

    public void Hide()
    {
        active = false;
        panel.SetActive(false);
        player.SetInMenu(false);
        CameraMovement.instance.SetBlock(false);
        book = null;
    }

    //void Update()
    //{
    //    if(!active && Input.GetKeyDown(KeyCode.B))
    //    {
    //        Show();
    //    }
    //}

    public bool GetActive()
    {
        return active;
    }

    public void RightBtn()
    {
        if (book == null)
            return;
        if (book.pages.Count == 0)
            return;
        pageIndex += 1;
        if(pageIndex >= book.pages.Count)
            pageIndex = book.pages.Count - 1;
        screenText.text = book.pages[pageIndex].text;
        pageText.text = $"{pageIndex + 1}/{book.pages.Count}";
    }

    public void LeftBtn()
    {
        if (book == null)
            return;
        if (book.pages.Count == 0)
            return;
        pageIndex -= 1;
        if (pageIndex <= 0)
            pageIndex = 0;
        screenText.text = book.pages[pageIndex].text;
        pageText.text = $"{pageIndex + 1}/{book.pages.Count}";
    }
}
