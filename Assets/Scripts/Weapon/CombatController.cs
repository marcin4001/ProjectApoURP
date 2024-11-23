using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CombatController : MonoBehaviour
{
    public static CombatController instance;
    [SerializeField] private EnemyController[] enemies;
    [SerializeField] private int currentIndex = 0;
    [SerializeField] private int actionPoint = 2;
    [SerializeField] private int actionPointMax = 2;
    [SerializeField] private GameObject bloodPrefab; 
    private PlayerController player;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
    }

    public void StartCombat(bool firstPlayer)
    {
        Debug.Log("StartCombat");
        if (enemies.Length == 0)
            return;
        GameParam.instance.inCombat = true;
        HUDController.instance.ShowFightPanel();
        if(firstPlayer)
        {
            currentIndex = -1;
            actionPoint = actionPointMax;
        }
        else
        {
            player.SetBlock(true);
            currentIndex = 0;
            enemies[currentIndex].StartTurn();
        }
    }

    public void NextTurn()
    {
        Debug.Log("NextTurn");
        if (PlayerStats.instance.isDeath())
        {
            GameParam.instance.inCombat = false;
            HUDController.instance.HideFightPanel();
            StartCoroutine(AfterDeath());
            return;
        }
        if (isEndCombat())
        {
            GameParam.instance.inCombat = false;
            HUDController.instance.HideFightPanel();
            player.SetBlock(false);
            enemies = new EnemyController[0];
            return;
        }
        currentIndex++;
        if (currentIndex >= enemies.Length)
            currentIndex = -1;
        if(currentIndex < 0)
        {
            player.SetBlock(false);
            actionPoint = actionPointMax;
        }
        else
        {
            player.SetBlock(true);
            enemies[currentIndex].StartTurn();
        }
    }

    public void RemoveAP(int point)
    {
        actionPoint -= point;
        if (actionPoint == 1)
        {
            HUDController.instance.AddConsolelog("You have one action point left");
            HUDController.instance.AddConsolelog("left.");
        }
        if(actionPoint <= 0)
        {
            //HUDController.instance.AddConsolelog("Your turn is over.");
            actionPoint = 0;
            NextTurn();
        }
    }

    public void SetGroup(EnemyGroup group)
    {
        enemies = group.GetEnemies();
    }

    public int CalculateDamege(int baseDamage, int chanceToHit, int chanceToCrit, out bool isCrit)
    {
        isCrit = false;
        int hitChance = Random.Range(0, 100);
        if(hitChance > chanceToHit)
        {
            return 0;
        }
        int critChance = Random.Range(0, 100);
        if (critChance > chanceToCrit)
        {
            return baseDamage;
        }
        else
        {
            isCrit = true;
            return baseDamage * 2;
        }
    }

    private IEnumerator AfterDeath()
    {
        yield return new WaitForSeconds(2f);
        Vector3 spawnBlood = player.transform.position - player.transform.forward * 1f;
        spawnBlood.y = 0.01f;
        Instantiate(bloodPrefab, spawnBlood, Quaternion.identity);
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene(0);
    }

    private bool isEndCombat()
    {
        bool result = true;
        foreach(EnemyController enemy in enemies)
        {
            result &= enemy.IsDeath();
        }
        return result;
    }
}
