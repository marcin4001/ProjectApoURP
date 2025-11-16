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
    [SerializeField] private Vector3[] slots;
    [SerializeField] private string deathSceneName = "DeathScene";
    [SerializeField] private GameObject slotDebug;
    [SerializeField] private bool skipTurnPlayer = false;
    private PlayerController player;
    private List<GameObject> debugSlots = new List<GameObject>();
    private bool getDamage;

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
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].SetIndexSlot(i);
        }
        GameParam.instance.inCombat = true;
        HUDController.instance.ShowFightPanel();
        if(firstPlayer)
        {
            Debug.Log("firstPlayer");
            if (!CameraMovement.instance.ObjectInFov(player.transform))
                CameraMovement.instance.CenterCameraToPlayer();
            currentIndex = -1;
            actionPoint = actionPointMax;
            foreach(EnemyController enemy in enemies)
                enemy.SetActiveAgent(false);
        }
        else
        {
            CreateSlots();
            player.SetBlock(true);
            foreach (EnemyController enemy in enemies)
                enemy.SetActiveAgent(true);
            currentIndex = 0;
            enemies[currentIndex].StartTurn();
        }
    }

    public void StopCombat()
    {
        Debug.Log("StopCombat");
        GameParam.instance.inCombat = false;
        HUDController.instance.HideFightPanel();
    }

    public void SkipTurnPlayer()
    {
        if (currentIndex < 0)
        {
            skipTurnPlayer = true;
            NextTurn();
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
            if (!CameraMovement.instance.ObjectInFov(player.transform))
                CameraMovement.instance.CenterCameraToPlayer();
            foreach (EnemyController enemy in enemies)
                enemy.SetActiveAgent(false);
            player.SetBlock(false);
            actionPoint = actionPointMax;
        }
        else
        {
            if(currentIndex == 0)
                CreateSlots();
            player.SetBlock(true);
            foreach (EnemyController enemy in enemies)
                enemy.SetActiveAgent(true);
            enemies[currentIndex].StartTurn();
        }
    }

    public void RemoveAP(int point)
    {
        actionPoint -= point;
        if (actionPoint == 1)
        {
            HUDController.instance.AddConsolelog("You have one action point");
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

    public bool IsSkipTurnPlayer()
    {
        return skipTurnPlayer;
    }

    public void UnsetSkipTurnPlayer()
    {
        skipTurnPlayer = false;
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

    public int CalculateDamegePlayer(int baseDamage, out bool isCrit)
    {
        isCrit = false;
        int hitChance = Random.Range(0, 100);
        Debug.Log("Hit Chance: " + PlayerStats.instance.GetHitChance() + " Random Number: " + hitChance);
        
        if (hitChance > PlayerStats.instance.GetHitChance())
        {
            return 0;
        }
        int critChance = Random.Range(0, 100);
        if (critChance > GameParam.instance.chanceToCrit)
        {
            return baseDamage;
        }
        else
        {
            isCrit = true;
            return baseDamage * 2;
        }
    }

    public int CalculateDamegePlayerMelee(int baseDamage, out bool isCrit)
    {
        Debug.Log("Melee Damage");
        isCrit = false;
        int hitChance = Random.Range(0, 100);
        if (hitChance > GameParam.instance.chanceToHit)
        {
            return 0;
        }
        int critChance = Random.Range(0, 100);
        if (critChance > GameParam.instance.chanceToCrit)
        {
            return baseDamage;
        }
        else
        {
            isCrit = true;
            return baseDamage * 2;
        }
    }

    public int CalculateDamegePlayerOnlyCrit(int baseDamage, out bool isCrit)
    {
        isCrit = false;
        int critChance = Random.Range(0, 100);
        if (critChance > GameParam.instance.chanceToCrit)
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
        yield return new WaitForSeconds(3f);
        FadeController.instance.SetFadeIn(true);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(deathSceneName);
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

    private void CreateSlots()
    {
        slots = new Vector3[4];
        for (int i = 0; i < slots.Length; i++)
        {
            float angle = i * 90f;
            float x = Mathf.Cos(Mathf.Deg2Rad * angle) * 0.5f;
            float y = 0f;
            float z = Mathf.Sin(Mathf.Deg2Rad * angle) * 0.5f;
            slots[i] = new Vector3(x, y, z);
        }
        if (slotDebug == null)
            return;
        foreach(GameObject debugSlot in debugSlots)
        {
            Destroy(debugSlot);
        }
        debugSlots.Clear();
        foreach(Vector3 slot in slots)
        {
            GameObject debugSlot = Instantiate(slotDebug, player.transform.position + slot, Quaternion.identity);
            debugSlots.Add(debugSlot);
        }
    }

    public Vector3 GetSlot(int index)
    {
        return slots[index] + player.transform.position;
    }

    public bool IsGetDamge()
    {
        return getDamage;
    }

    public void SetGetDamage(bool value)
    {
        getDamage = value;
    }

    public GameObject GetBloodPrefab()
    {
        return bloodPrefab;
    }
}
