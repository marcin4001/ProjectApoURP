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
        GameParam.instance.inCombat = true;
        enemies = FindObjectsByType<EnemyController>(FindObjectsSortMode.None);
        if(firstPlayer)
        {
            currentIndex = -1;
        }
        else
        {
            player.SetBlock(true);
            currentIndex = 0;
            Debug.Log("StartCombat");
            enemies[currentIndex].StartTurn();
        }
    }

    public void NextTurn()
    {
        Debug.Log("NextTurn");
        if (PlayerStats.instance.isDeath())
        {
            GameParam.instance.inCombat = false;
            StartCoroutine(AfterDeath());
            return;
        }
        if (isEndCombat())
        {
            GameParam.instance.inCombat = false;
            player.SetBlock(false);
            return;
        }
        currentIndex++;
        if (currentIndex >= enemies.Length)
            currentIndex = -1;
        if(currentIndex < 0)
        {
            player.SetBlock(false);
        }
        else
        {
            player.SetBlock(true);
            enemies[currentIndex].StartTurn();
        }
    }

    private IEnumerator AfterDeath()
    {
        yield return new WaitForSeconds(5f);
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

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F3))
        {
            if (!GameParam.instance.inCombat)
            {
                StartCombat(false);
            }
        }
    }
}
