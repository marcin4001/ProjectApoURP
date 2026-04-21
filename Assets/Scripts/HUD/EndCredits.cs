using UnityEngine;

public class EndCredits : MonoBehaviour
{
    [SerializeField] private RectTransform textTransform;
    [SerializeField] private float speed = 10;
    [SerializeField] private float endingYPos = 3520f;
    [SerializeField] private int indexTheme = 1;
    void Start()
    {
        MusicManager.instance.SetMaxVolume(GameParam.instance.maxVolumeTheme);
        MusicManager.instance.SetTheme(indexTheme);
    }

    
    void Update()
    {
        Vector2 position = textTransform.anchoredPosition;
        position.y += Time.deltaTime * speed;
        textTransform.anchoredPosition = position;

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Exit");
            Application.Quit();
        }

        if(position.y >= endingYPos)
        {
            Debug.Log("Exit");
            Application.Quit();
        }
    }
}
