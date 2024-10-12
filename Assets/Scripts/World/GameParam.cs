using UnityEngine;

public class GameParam : MonoBehaviour
{
    public static GameParam instance;
    public int day = 1;
    public float currentTime;
    public int healthPoint = 100;
    public int healthPointMax = 100;
    public int radLevel = 0;
    public int radLevelMax = 5;
    public Vector2 mapPosition = Vector2.zero;
    public string prevScene = "";
    public MapSign[] mapSigns;
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

    public MapSignState GetMapSignState(string _name)
    {
        foreach(MapSign sign in mapSigns) 
        {
            if (sign.name == _name)
                return sign.state;
        }
        return MapSignState.Hidden;
    }

    public void SetMapSignState(string _name, MapSignState state)
    {
        foreach (MapSign sign in mapSigns)
        {
            if(sign.name == _name)
                sign.state = state;
        }
    }

    public void AddDay()
    {
        day = day + 1;
    }
}
