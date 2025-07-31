using UnityEngine;

public class GameParam : MonoBehaviour
{
    public static GameParam instance;
    public int day = 1;
    public float currentTime;
    public int healthPoint = 100;
    public int healthPointMax = 100;
    public int healthPointMaxBase = 100;
    public int radLevel = 0;
    public int radLevelMax = 5;
    public int exp = 0;
    public int level = 0;
    public int expToNextLevel = 500;
    public bool inCombat = false;
    public Vector2 mapPosition = Vector2.zero;
    public string prevScene = "";
    public MapSign[] mapSigns;
    public Vector3 cameraPivotRot = new Vector3(30f, 35f, 0f);
    public float nearClip = -15f;
    public float cutoutSize = 0.2f;
    public Vector3 posCamera = new Vector3(0f, 0f, -20f);
    public bool showGrid = false;
    public bool startGame;
    public int chanceToHit = 70;
    public int chanceToCrit = 20;
    public float maxVolumeTheme = 1f;
    [Range(0f, 1f)] public float mainMusicVolume = 1f;
    [Range(0f, 1f)] public float sfxVolume = 1f;
    public Vector2Int[] resolutions;
    public int currentResolution;
    public Color normalDialogueTag;
    public Color angryDialogueTag;
    public bool inDev = false;
    public bool inDemo = false;
    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        mainMusicVolume = PlayerPrefs.GetFloat("mainMusicVolume", 1f);
        sfxVolume = PlayerPrefs.GetFloat("sfxVolume", 1f);
    }

    public void UpdateParam()
    {
        if(TimeGame.instance != null)
            currentTime = TimeGame.instance.GetCurrentTime();
        if(PlayerStats.instance != null)
        {
            healthPoint = PlayerStats.instance.GetHP();
            healthPointMax = PlayerStats.instance.GetMaxHP();
            healthPointMaxBase = PlayerStats.instance.GetMaxHPBase();
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
        healthPointMaxBase = 100;
        radLevel = 0;
        radLevelMax = 5;
        exp = 0;
        level = 0;
        expToNextLevel = 500;
        prevScene = "";
        foreach(MapSign sign in mapSigns)
        {
            sign.state = MapSignState.Hidden;
        }
        mapSigns[0].state = MapSignState.Explored;
        mapPosition = new Vector2(-227.5f, 227.5f);
    }

    public void AddDay()
    {
        day = day + 1;
    }

    public void AddExp(int _exp)
    {
        exp += _exp;
        HUDController.instance.AddConsolelog($"You earn {_exp} EXP.");
        while(exp >= expToNextLevel)
        {
            LevelUp();
        }
    }

    public void LevelUp()
    {
        exp -= expToNextLevel;
        level += 1;
        expToNextLevel += 500;
        HUDController.instance.AddConsolelog($"Level Up! New level: {level}");
    }

}
