using UnityEngine;

public class LoadingPanel : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    void Start()
    {
        panel.SetActive(false);
    }

    public void Show()
    {
        panel.SetActive(true);
    }
}
