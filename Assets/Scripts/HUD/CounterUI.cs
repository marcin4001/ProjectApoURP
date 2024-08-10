using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CounterUI : MonoBehaviour
{
    public static CounterUI instance;
    [SerializeField] private int maxNumber = 100;
    [SerializeField] private int number = 100;
    [SerializeField] private bool apply = true;
    [SerializeField] private TextMeshProUGUI counterText;
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;
    [SerializeField] private Button button1;
    [SerializeField] private Button button50;
    [SerializeField] private Button button100;
    [SerializeField] private Button applyButton;
    private Canvas canvas;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        canvas = GetComponent<Canvas>();
        leftButton.onClick.AddListener(OnLeftButtonClick);
        rightButton.onClick.AddListener(OnRightButtonClick);
        button1.onClick.AddListener(OnButton1Click);
        button50.onClick.AddListener(OnButton50Click);
        button100.onClick.AddListener(OnButton100Click);
        applyButton.onClick.AddListener(OnApplyClick);
        canvas.enabled = false;
    }

    public void Show(int _maxNumber)
    {
        apply = false;
        maxNumber = _maxNumber;
        number = _maxNumber;
        counterText.text = number.ToString("D5");
        canvas.enabled = true;
        EscController.instance.SetBlock(true);
    }

    private void OnRightButtonClick()
    {
        number += 1;
        if(number > maxNumber)
            number = maxNumber;
        counterText.text = number.ToString("D5");
    }

    private void OnLeftButtonClick()
    {
        number -= 1;
        if(number < 1)
            number = 1;
        counterText.text = number.ToString("D5");
    }

    private void OnButton1Click()
    {
        number = 1;
        counterText.text = number.ToString("D5");
    }

    private void OnButton50Click()
    {
        number = maxNumber / 2;
        counterText.text = number.ToString("D5");
    }

    private void OnButton100Click()
    {
        number = maxNumber;
        counterText.text = number.ToString("D5");
    }

    private void OnApplyClick()
    {
        apply = true;
        canvas.enabled = false;
        EscController.instance.SetBlock(false);
    }

    public int GetNumber()
    {
        return number; 
    }
    
    public bool IsApply()
    {
        return apply;
    }
}
