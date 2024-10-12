using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats instance;
    [SerializeField] private int healthPoint = 100;
    [SerializeField] private int healthPointMax = 100;
    [SerializeField] private int radLevel = 0;
    [SerializeField] private int radLevelMax = 5;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if(GameParam.instance != null)
        {
            healthPoint = GameParam.instance.healthPoint;
            healthPointMax = GameParam.instance.healthPointMax;
            radLevel = GameParam.instance.radLevel;
            radLevelMax = GameParam.instance.radLevelMax;
        }
        HUDController.instance.UpdateHPBar(healthPoint, healthPointMax);
        HUDController.instance.UpdateRadBar(radLevel, radLevelMax);
    }

    public void AddHealthPoint(int point)
    {
        healthPoint += point;
        if(healthPoint > healthPointMax)
            healthPoint = healthPointMax;
        HUDController.instance.UpdateHPBar(healthPoint, healthPointMax);

    }

    public void RemoveHealthPoint(int point)
    {
        healthPoint -= point;
        if (healthPoint < 0)
            healthPoint = 0;
        HUDController.instance.UpdateHPBar(healthPoint, healthPointMax);
    }

    public void SetHealthPointMax(int newMax)
    {
        healthPointMax = newMax;
        healthPoint = newMax;
        HUDController.instance.UpdateHPBar(healthPoint, healthPointMax);
    }

    public bool isDeath()
    {
        return healthPoint <= 0;
    }

    public void AddOneRadLevel()
    {
        radLevel++;
        if (radLevel > radLevelMax)
            radLevel = radLevelMax;
        HUDController.instance.UpdateRadBar(radLevel, radLevelMax);
    }

    public void RemoveOneRadLevel()
    {
        radLevel--;
        if (radLevel < 0)
            radLevel = 0;
        HUDController.instance.UpdateRadBar(radLevel, radLevelMax);
    }

    public void RemoveAllLevelsRad()
    {
        radLevel = 0;
        HUDController.instance.UpdateRadBar(radLevel, radLevelMax);
    }

    public string GetStatsText()
    {
        return $"Name: Revo\nHealth: {healthPoint}/{healthPointMax}\nRadiation level: {radLevel}/{radLevelMax}\nDay: {GameParam.instance.day}";
    }

    public int GetHP()
    {
        return healthPoint;
    }

    public int GetMaxHP()
    {
        return healthPointMax;
    }

    public int GetRadLevel()
    {
        return radLevel;
    }

    public int GetMaxRadLevel()
    {
        return radLevelMax;
    }
}
