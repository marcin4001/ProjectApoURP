using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueOptionButton : MonoBehaviour
{
    [SerializeField] private DialogueOption option;
    private Button button;
    private TextMeshProUGUI textOption;

    public void Init(DialogueOption _option)
    {
        option = _option;
        button = GetComponent<Button>();
        textOption = GetComponent<TextMeshProUGUI>();
        button.onClick.AddListener(OnClick);
        textOption.text = $"■ {option.optionText}";
    }

    private void OnClick()
    {
        foreach(ActionDialogue action in option.actions)
        {
            action.Execute();
        }
        DialogueController.instance.ShowNextDialogue(option);
    }
}
