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
    public bool inCombat = false;
    public Vector2 mapPosition = Vector2.zero;
    public string prevScene = "";
    public MapSign[] mapSigns;
    public Vector3 cameraPivotRot = new Vector3(30f, 35f, 0f);
    public bool startGame;
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

    public void SetStartParam()
    {
        startGame = true;
        currentTime = 6;
        day = 1;
        healthPoint = 100;
        healthPointMax = 100;
        radLevel = 0;
        radLevelMax = 5;
        prevScene = "";
        foreach(MapSign sign in mapSigns)
        {
            sign.state = MapSignState.Hidden;
        }
        mapSigns[0].state = MapSignState.Explored;
    }

    public void AddDay()
    {
        day = day + 1;
    }
}
