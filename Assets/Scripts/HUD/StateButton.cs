using UnityEngine;
using UnityEngine.UI;

public class StateButton : MonoBehaviour
{
    [SerializeField] private Image selectFrame;
    [SerializeField] private PlayerActionState state;
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        HUDController.instance.SetStatePlayer(state);
    }

    public void HideSelectFrame()
    {
        selectFrame.enabled = false;
    }

    public void ShoeSelectFrame()
    {
        selectFrame.enabled = true;
    }
}
