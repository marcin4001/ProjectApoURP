using UnityEngine;

public class GameParam : MonoBehaviour
{
    public static GameParam instance;
    public float currentTime;
    public int healthPoint = 100;
    public int healthPointMax = 100;
    public int radLevel = 0;
    public int radLevelMax = 5;
    public Vector2 mapPosition = Vector2.zero;
    public string prevScene = "";
    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void UpdateParam()
    {
        if(TimeGame.instance != null)
            currentTime = TimeGame.instance.GetCurrentTime();
        if(PlayerStats.instance != null)
        {
            healthPoint = PlayerStats.instance.GetHP();
            healthPointMax = PlayerStats.instance.GetMaxHP();
            radLevel = PlayerStats.instance.GetRadLevel();
            radLevelMax = PlayerStats.instance.GetMaxRadLevel();
        }
    }
}
