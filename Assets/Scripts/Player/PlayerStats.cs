using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats instance;
    [SerializeField] private int healthPoint = 100;
    [SerializeField] private int healthPointMax = 100;
    [SerializeField] private int healthPointMaxBase = 100;
    [SerializeField] private int radLevel = 0;
    [SerializeField] private int radLevelMax = 5;
    [SerializeField] private int strength = 5;
    [SerializeField] private int dexterity = 5;
    [SerializeField] private int technical = 5;
    [SerializeField] private int perception = 5;
    [SerializeField] private int baseHandDamage = 3;
    [SerializeField] private int baseHitChance = 20;
    [SerializeField] private int baseDodgeChance = 10;
    [SerializeField] private int baseCritChance = 5;
    [SerializeField] private int baseLockpickChance = 25;
    [SerializeField] private int baseRepairChance = 10;
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
            strength = GameParam.instance.strength;
            dexterity = GameParam.instance.dexterity;
            technical = GameParam.instance.technical;
            perception = GameParam.instance.perception;
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
        int prevHpMax = healthPointMax;
        healthPointMaxBase = newMax;
        UpdateMaxHP();
        if (healthPoint >= prevHpMax)
            healthPoint = healthPointMax;
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
            $"Armor defense: {armorDef}\n" +
            $"STR:{strength}\tDEX:{dexterity}\n" +
            $"TEC:{technical}\tPER:{perception}";
    }

    public void UpdateStatsInGameParam()
    {
        GameParam.instance.strength = strength;
        GameParam.instance.dexterity = dexterity;
        GameParam.instance.technical = technical;
        GameParam.instance.perception = perception;
    }

    public int[] GetStatsAsArray()
    {
        int[] array = new int[4];
        array[0] = strength; 
        array[1] = dexterity;  
        array[2] = technical; 
        array[3] = perception;
        return array;
    }

    public int GetStrength()
    {
        return strength;
    }

    public int GetTechnical()
    {
        return technical;
    }

    public void SetStats(int[] statsArray)
    {
        if(statsArray.Length != 4)
            return;
        strength = statsArray[0];
        dexterity = statsArray[1];
        technical = statsArray[2];
        perception = statsArray[3];
        int healthBase = 50;
        int calculatedHealth = healthBase + (8 * (strength - 1));
        SetHealthPointMax(calculatedHealth);
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

    public int GetHandDamage()
    {
        return baseHandDamage + (Mathf.FloorToInt(strength/2) * 2);
    }

    public int GetHitChance()
    {
        return baseHitChance + (perception - 1) * 7;
    }

    public int GetDodgeChance()
    {
        return baseDodgeChance + (dexterity - 1) * (75 - 10) / 9;
    }

    public int GetCritChance()
    {
        return Mathf.RoundToInt(baseCritChance + (perception - 1) * (25 - 5) / 9f);
    }

    public int GetLockpickChance()
    {
        return Mathf.RoundToInt(baseLockpickChance + (dexterity - 1) * (95f - 25f) / 9f);
    }

    public int GetRepairChance()
    {
        return baseRepairChance + (technical - 1) * 9;
    }

    public void AddStrengthPoint()
    {
        if (strength >= 10)
            return;
        strength += 1;
        HUDController.instance.AddConsolelog("You gained +1 Strength");
        HUDController.instance.AddConsolelog("point");
        int healthBase = 50;
        int calculatedHealth = healthBase + (8 * (strength - 1));
        SetHealthPointMax(calculatedHealth);
    }

    public void AddDexterityPoint()
    {
        if (dexterity >= 10)
            return;
        dexterity += 1;
        HUDController.instance.AddConsolelog("You gained +1 Dexterity");
        HUDController.instance.AddConsolelog("point");
    }

    public void AddTechnicalPoint()
    {
        if(technical >= 10)
            return;
        technical += 1;
        HUDController.instance.AddConsolelog("You gained +1 Technical");
        HUDController.instance.AddConsolelog("point");
    }

    public void AddPerceptionPoint()
    {
        if (perception >= 10)
            return;
        perception += 1;
        HUDController.instance.AddConsolelog("You gained +1 Perception");
        HUDController.instance.AddConsolelog("point");
    }
}
