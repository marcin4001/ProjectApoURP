using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats instance;
    [SerializeField] private int healthPoint = 100;
    [SerializeField] private int healthPointMax = 100;
    [SerializeField] private int healthPointMaxBase = 100;
    [SerializeField] private int radLevel = 0;
    [SerializeField] private int radLevelMax = 5;
    private Dictionary<int, float> radHPPercent = new Dictionary<int, float>
    {
        {0, 1f},
        {1, 0.95f},
        {2, 0.9f},
        {3, 0.8f},
        {4, 0.7f},
        {5, 0.6f},
    };


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
            healthPointMaxBase = GameParam.instance.healthPointMaxBase;
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
        healthPointMaxBase = newMax;
        radLevel = 0;
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
        UpdateMaxHP();
        HUDController.instance.UpdateRadBar(radLevel, radLevelMax);
        HUDController.instance.UpdateHPBar(healthPoint, healthPointMax);
    }

    public void RemoveOneRadLevel()
    {
        radLevel--;
        if (radLevel < 0)
            radLevel = 0;
        UpdateMaxHP();
        HUDController.instance.UpdateRadBar(radLevel, radLevelMax);
        HUDController.instance.UpdateHPBar(healthPoint, healthPointMax);
    }

    public void RemoveAllLevelsRad()
    {
        radLevel = 0;
        UpdateMaxHP();
        HUDController.instance.UpdateRadBar(radLevel, radLevelMax);
        HUDController.instance.UpdateHPBar(healthPoint, healthPointMax);
    }

    private void UpdateMaxHP()
    {
        healthPointMax = Mathf.RoundToInt(healthPointMaxBase * radHPPercent[radLevel]);
        if(healthPoint > healthPointMax)
            healthPoint = healthPointMax;
    }

    public string GetStatsText(ArmorItem armor)
    {
        int armorDef = 0;
        if (armor != null)
        {
            armorDef = armor.defense;
        }
        return $"Name: Thomas\nHealth: {healthPoint}/{healthPointMax}\n" +
            $"Radiation level: {radLevel}/{radLevelMax}\nDay: {GameParam.instance.day}\n" +
            $"Level: {GameParam.instance.level}\nExp: {GameParam.instance.exp}/{GameParam.instance.expToNextLevel}\n" +
            $"Armor defense: {armorDef}";
    }

    public int GetHP()
    {
        return healthPoint;
    }

    public int GetMaxHP()
    {
        return healthPointMax;
    }

    public int GetMaxHPBase()
    {
        return healthPointMaxBase;
    }

    public int GetRadLevel()
    {
        return radLevel;
    }

    public int GetMaxRadLevel()
    {
        return radLevelMax;
    }


    public bool RadLevelIsFull()
    {
        return radLevel >= radLevelMax;
    }
}
