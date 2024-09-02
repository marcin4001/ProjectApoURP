using UnityEngine;
using UnityEngine.UI;

public class MapSceneManager : MonoBehaviour
{
    [SerializeField] private GameObject grid;
    [SerializeField] private Button gridButton;
    [SerializeField] private Button enterButton;
    [SerializeField] private Sprite enterBtnActiveSprite;
    [SerializeField] private Sprite enterBtnInactiveSprite;
    void Start()
    {
        grid.SetActive(false);
        gridButton.onClick.AddListener(ShowGrid);
        enterButton.onClick.AddListener(OnClickEnter);
    }

    private void ShowGrid()
    {
        if(grid.activeSelf)
        {
            grid.SetActive(false);
        }
        else
        {
            grid.SetActive(true);
        }
    }

    private void OnClickEnter()
    {

    }
}
